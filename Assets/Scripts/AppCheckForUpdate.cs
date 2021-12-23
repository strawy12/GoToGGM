using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.AppUpdate;
using Google.Play.Common;

public class AppCheckForUpdate : MonoBehaviour
{
    AppUpdateManager appUpdateManager;

    private void Awake()
    {
        appUpdateManager = new AppUpdateManager();
        StartCoroutine(CheckForUpdate());
    }

    private IEnumerator CheckForUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {

            }
            else
            {

            }

            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

            StartCoroutine(StopImmediateUpdate(appUpdateInfoResult, appUpdateOptions));
        }
    }

    IEnumerator StopImmediateUpdate(AppUpdateInfo appUpdateInfoOp_i, AppUpdateOptions appUpdateOptions_i)
    {
        var startUpdateResult = appUpdateManager.StartUpdate(appUpdateInfoOp_i, appUpdateOptions_i);

        yield return startUpdateResult;
    }
}
