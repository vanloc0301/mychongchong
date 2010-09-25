rd /s/q debug\addins
rd /s/q debug\Data


xcopy ..\..\..\base\SkyMap.Net.Core\Resources\*.addin debug\addins\core\ /y/r
xcopy ..\..\..\base\SkyMap.Net.Core\Resources\*.resources debug\addins\core\ /y/r

xcopy ..\..\..\base\SkyMap.Net.Workflow.Client\Resources\*.resources debug\addins\Workflow\ /y/r
xcopy ..\..\..\base\SkyMap.Net.Workflow.Client\Resources\*.addin debug\addins\Workflow\ /y/r

xcopy ..\SkyMap.Net.DataUtil\Resources\*.addin debug\addins\datautil\ /y/r
xcopy ..\SkyMap.Net.DataUtil\Resources\*.resources debug\addins\datautil\ /y/r
xcopy ..\SkyMap.Net.DataUtil\Resources\*.xml debug\addins\datautil\ /y/r
xcopy ..\SkyMap.Net.DataUtil\bin\debug\skymap.net.datautil.dll debug\addins\datautil\ /y/r

xcopy ..\..\..\base\SkyMap.Net.Workflow.Client\Resources\ControlConfig.xml debug\Data\Workflow\ /y/r

xcopy debug.cfg.xml debug\bin\Default.cfg.xml /y/r

xcopy configs\layouts\*.xml debug\data\Resources\layouts\ /y/r
xcopy configs\dataforms.config debug\bin\
