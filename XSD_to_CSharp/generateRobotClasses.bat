set XSD="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\xsd.exe"

set ROBOT_SCHEMA=robot.xsd
set STATE_DATA_SCHEMA=stateData.xsd

set OUTPUT_RELATIVE_DIR=..\FRCrobotCodeGen302 

call %XSD% %ROBOT_SCHEMA% /c /out:%OUTPUT_RELATIVE_DIR%
call %XSD% %STATE_DATA_SCHEMA% /c /out:%OUTPUT_RELATIVE_DIR%
