﻿using System;
using System.Collections.Generic;
using System.Linq;
using Backups;
using BackupsExtra.Exception;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public class RemoveByHybrid : IRemoveRestorePoint
    {
        private readonly DateTime _dateCheck = DateTime.Now.Date.AddDays(-3);
        private readonly int _countCheck = 2;

        public List<RestorePoint> RemoveRestorePoint(
            IRestorePointRemover restorePointRemover,
            ComplementedBackupJob backupJob,
            bool isTimecodeOn)
        {
            var resultRemoveList = new List<RestorePoint>();
            if (backupJob.GetIsAllLimitsOn())
            {
                var restorePointsToDelete = backupJob.GetNewRestorePoints()
                    .Where(restorePoint => restorePoint.CreationTime.Date <= _dateCheck).ToList();

                if (restorePointsToDelete.Count == backupJob.GetNewRestorePoints().Count)
                {
                    throw new BackupsExtraException(
                        $"The count of restore points is {backupJob.GetNewRestorePoints().Count}, you can't remove all of them!");
                }

                var counter = 0;
                var restorePointsToDelete2 = new List<RestorePoint>();
                foreach (var restorePoint in backupJob.GetNewRestorePoints())
                {
                    if (backupJob.GetNewRestorePoints().Count - counter == _countCheck)
                    {
                        break;
                    }

                    restorePointsToDelete2.Add(restorePoint);
                    ++counter;
                }

                resultRemoveList = (
                    from restorePoint1 in restorePointsToDelete
                    from restorePoint2 in restorePointsToDelete2
                    where restorePoint1 == restorePoint2
                    select restorePoint1).ToList();
            }
            else
            {
                var restorePointsToDelete = backupJob.GetNewRestorePoints()
                    .Where(restorePoint => restorePoint.CreationTime.Date <= _dateCheck).ToList();

                if (restorePointsToDelete.Count == backupJob.GetNewRestorePoints().Count)
                {
                    throw new BackupsExtraException(
                        $"The count of restore points is {backupJob.GetNewRestorePoints().Count}, you can't remove all of them!");
                }

                var counter = 0;
                var restorePointsToDelete2 = new List<RestorePoint>();
                foreach (var restorePoint in backupJob.GetNewRestorePoints())
                {
                    if (counter == _countCheck)
                    {
                        break;
                    }

                    restorePointsToDelete2.Add(restorePoint);
                    ++counter;
                }

                resultRemoveList = restorePointsToDelete;
                resultRemoveList.AddRange(
                    from restorePoint1 in restorePointsToDelete
                    from restorePoint2 in restorePointsToDelete2
                    where restorePoint1 != restorePoint2
                    select restorePoint2);
            }

            return restorePointRemover.RemoveRestorePoint(backupJob, resultRemoveList, isTimecodeOn);
        }
    }
}