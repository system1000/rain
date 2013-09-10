using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CloudBooks
{
    public class FileListFragment : ListFragment
    {
        public static readonly string DefaultInitialDirectory = "/";
        public static Action<Action> A_GoBack;
        public string CurrentPath = string.Empty;
        private FileListAdapter _adapter;
        private DirectoryInfo _directory;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _adapter = new FileListAdapter(Activity, new FileSystemInfo[0]);
            ListAdapter = _adapter;
            A_GoBack = GoBack;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var fileSystemInfo = _adapter.GetItem(position);

            if (fileSystemInfo.IsFile())
            {
                Log.Verbose("FileListFragment", "The file {0} was clicked.", fileSystemInfo.FullName);
                Toast.MakeText(Activity, "You selected file " + fileSystemInfo.FullName, ToastLength.Short).Show();
            }
            else
            {
                RefreshFilesList(fileSystemInfo.FullName);
            }

            base.OnListItemClick(l, v, position, id);
        }

        public override void OnResume()
        {
            base.OnResume();
            RefreshFilesList(DefaultInitialDirectory);
        }


        public void GoBack(Action v)
        {
            if (!string.IsNullOrEmpty(CurrentPath) && CurrentPath != DefaultInitialDirectory)
            {
                string path = CurrentPath.Substring(0, CurrentPath.LastIndexOf("/"));
                if (!string.IsNullOrEmpty(path))
                {
                    RefreshFilesList(path);
                }
                else
                {
                    RefreshFilesList("/");
                }
            }
            else 
            {
                v();
            }
        }

        public void RefreshFilesList(string directory)
        {
            CurrentPath = directory;
            IList<FileSystemInfo> visibleThings = new List<FileSystemInfo>();
            var dir = new DirectoryInfo(directory);

            try
            {
                foreach (var item in dir.GetFileSystemInfos().Where(item => item.IsVisible()))
                {
                    visibleThings.Add(item);
                }
            }
            catch (Exception ex)
            {
                Log.Error("FileListFragment", "Couldn't access the directory " + _directory.FullName + "; " + ex);
                Toast.MakeText(Activity, "Problem retrieving contents of " + directory, ToastLength.Long).Show();
                return;
            }

            _directory = dir;

            _adapter.AddDirectoryContents(visibleThings);

            // If we don't do this, then the ListView will not update itself when then data set 
            // in the adapter changes. It will appear to the user that nothing has happened.
            ListView.RefreshDrawableState();

            Log.Verbose("FileListFragment", "Displaying the contents of directory {0}.", directory);
        }
    }
}