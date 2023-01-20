set XSD="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\xsd.exe"
set ClassGenerator=.\XmlSchemaClassGenerator.2.0.732\XmlSchemaClassGenerator.Console.exe

rem --output=c:\FRC\temp --pascal- C:\FRC\302CodeGenerator\XSD_to_CSharp\robot.xsd

set ROBOT_FILENAME=robot
set STATE_DATA_FILENAME=stateData

set ROBOT_SCHEMA=%ROBOT_FILENAME%.xsd
set STATE_DATA_SCHEMA=%STATE_DATA_FILENAME%.xsd

set ROBOT_CSHARP=%ROBOT_FILENAME%.cs
set STATE_DATA_CSHARP=%STATE_DATA_FILENAME%.cs

set OUTPUT_RELATIVE_DIR=..\robotConfiguration

call %ClassGenerator% --output=%OUTPUT_RELATIVE_DIR% --pascal- --nullable --useShouldSerialize %ROBOT_SCHEMA%
rem call %XSD% %ROBOT_SCHEMA% /c /out:%OUTPUT_RELATIVE_DIR%
call %XSD% %STATE_DATA_SCHEMA% /c /out:%OUTPUT_RELATIVE_DIR%

call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%/%ROBOT_CSHARP% "[System.ComponentModel.DefaultValueAttribute(" "//[System.ComponentModel.DefaultValueAttribute("
rem call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%/%ROBOT_CSHARP% "[System.Xml.Serialization.XmlTypeAttribute(" "//[System.Xml.Serialization.XmlTypeAttribute("
rem call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%/%ROBOT_CSHARP% "[System.Xml.Serialization.XmlRootAttribute(" "//[System.Xml.Serialization.XmlRootAttribute("


call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%ROBOT_CSHARP% "[System.Xml.Serialization.XmlTypeAttribute(" "//[System.Xml.Serialization.XmlTypeAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%ROBOT_CSHARP% "[System.Xml.Serialization.XmlRootAttribute(" "//[System.Xml.Serialization.XmlRootAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%ROBOT_CSHARP% "[System.ComponentModel.DataAnnotations.RequiredAttribute()]" "//[System.ComponentModel.DataAnnotations.RequiredAttribute()]"

