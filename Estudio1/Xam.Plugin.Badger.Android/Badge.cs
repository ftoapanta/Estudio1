﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Content.PM;
using Xam.Plugin.Badger.Droid.Implementation;

namespace Xam.Plugin.Badger.Droid
{
    public class Badge
    {
        static BaseBadge mBadge;
        internal static Context mContext;
        static string LOG_TAG = "BadgeImplementation";
        static List<String> BADGERS = new List<String>(
            new String[] 
            {
                "AdwHomeBadger",
                "ApexHomeBadger",
                "AsusHomeLauncher",
                "HuaweiHomeBadger",
                "LGHomeBadger",
                "NewHtcHomeBadger",
                "NovaHomeBadger",
                "SamsungHomeBadger",
                "SolidHomeBadger",
                "SonyHomeBadger",
                "XiaomiHomeBadger",
            }
        );

        public Badge(Context context)
        {
            GetBadger(mContext = context);
        }

        public int Count
        {
            get
            {
                return mBadge.GetCount();
            }
            set
            {
                try
                {
                    mBadge.SetCount(value);
                }
                catch (Exception e)
                {
                    Log.Error(LOG_TAG, e.Message, e);
                }
            }
        }

        public void Increment()
            => mBadge.Increment();

        public void SetCount(int badgeCount)
            => mBadge.SetCount(badgeCount);

        internal BaseBadge GetBadger(Context context)
        {
            if (mBadge != null)
            {
                return mBadge;
            }
            Log.Debug(LOG_TAG, "Finding badger");

            try
            {
                Intent intent = new Intent(Intent.ActionMain);
                intent.AddCategory(Intent.CategoryHome);
                ResolveInfo resolveInfo = context.PackageManager.ResolveActivity(intent, PackageInfoFlags.MatchDefaultOnly);
                String currentHomePackage = resolveInfo.ActivityInfo.PackageName.ToLower();

                /*if (Build.Manufacturer.ToLower() == "xiaomi") {
					mBadge = new XiaomiHomeBadger(context);
					return mBadge;
				}*/

                foreach (string badgeclass in BADGERS)
                {
                    Type t = Type.GetType("Xam.Plugin.Badger.Droid.Implementation." + badgeclass);
                    BaseBadge myObject = (BaseBadge)Activator.CreateInstance(t, true);
                    if (myObject.GetSupportLaunchers().Contains(currentHomePackage))
                    {
                        myObject.SetContext(context);
                        mBadge = myObject;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(LOG_TAG, ex.Message.ToString());
            }

            if (mBadge == null)
            {
                mBadge = new DefaultBadger(context);
            }

            Log.Debug(LOG_TAG, "Returning badger:" + mBadge.ToString());
            return mBadge;
        }
    }
}