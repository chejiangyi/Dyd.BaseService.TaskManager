%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe E:\working\BasicService\Dyd.BaseService.TaskManager\Dyd.BaseService.TaskManager.WinService\Dyd.BaseService.TaskManager.WinService.exe
Net Start NodeService
sc config NodeService start= auto