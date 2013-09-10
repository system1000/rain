using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CloudBooks
{
    [Activity(Label = "FileActivity")]
    public class SearchFile : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SearchFileMain);
            // Create your application here
            var backBtn = FindViewById(Resource.Id.Go_Back);
            if (backBtn != null) 
            {
                backBtn.Click += (o, e) =>
                {
                    if (FileListFragment.A_GoBack != null) 
                    {
                        FileListFragment.A_GoBack(() => 
                        {
                            StartActivity(typeof(Activity1));
                            this.Finish();
                        });
                    }
                };
            }
        }


        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back) 
            {
                StartActivity(typeof(Activity1));
                this.Finish();
                return false;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}