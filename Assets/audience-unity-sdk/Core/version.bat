@echo off
echo Runing audience unity sdk version.bat

set file_path=version.txt
set feature=0
set major=0
set sprint=0
set commit_count=0
set branch_name=Branch
set commit_id=000000
for  /f "delims=]" %%i in (%file_path%) do (
    echo %%i | findstr /IC:"AUDIENCE_UNITY_SDK_FEATURE " > nul && set feature_str=%%i
    echo %%i | findstr /IC:"AUDIENCE_UNITY_SDK_MAJOR " > nul && set major_str=%%i
    echo %%i | findstr /IC:"AUDIENCE_UNITY_SDK_SPRINT " > nul && set sprint_str=%%i
)


for /f "tokens=2" %%i in ("%feature_str%") do (set feature=%%i)
for /f "tokens=2" %%i in ("%major_str%") do (set major=%%i)
for /f "tokens=2" %%i in ("%sprint_str%") do (set sprint=%%i)


for /F %%i in ('git rev-parse --short HEAD') do ( set commit_id=%%i)
for /F %%i in ('git rev-list HEAD --count') do ( set commit_count=%%i)
for /F %%i in ('git name-rev --name-only HEAD') do ( set branch_path=%%i)
for  %%i in (%branch_path%) do ( set branch_name=%%~ni)


echo AUDIENCE_UNITY_SDK_FEATURE %feature% > %file_path%
echo AUDIENCE_UNITY_SDK_MAJOR %major% >> %file_path%
echo AUDIENCE_UNITY_SDK_SPRINT %sprint% >> %file_path%
echo AUDIENCE_UNITY_SDK_COMMIT_COUNT %commit_count% >> %file_path%
echo AUDIENCE_UNITY_SDK_BRANCH_NAME "%branch_name%" >> %file_path%
echo AUDIENCE_UNITY_SDK_COMMIT_ID "%commit_id%" >> %file_path%

echo Audience Unity SDK Version : %feature%.%major%.%sprint%.%commit_count%
echo Audience Unity SDK Branch : %branch_name%
echo Audience Unity SDK Commit id : %commit_id%

exit /b 0