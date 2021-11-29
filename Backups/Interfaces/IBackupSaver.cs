﻿using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IBackupSaver
    {
        // virtual or local
        void SaveBackup(List<Repository> repositories, FileSystem fileSystem, RestorePoint restorePoint);
    }
}