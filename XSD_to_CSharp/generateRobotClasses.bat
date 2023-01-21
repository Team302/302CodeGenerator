set ClassGenerator=.\XmlSchemaClassGenerator.2.0.732\XmlSchemaClassGenerator.Console.exe

set ROBOT_FILENAME=robot
set STATE_DATA_FILENAME=stateData

set ROBOT_SCHEMA=%ROBOT_FILENAME%.xsd
set STATE_DATA_SCHEMA=%STATE_DATA_FILENAME%.xsd

set ROBOT_CSHARP=%ROBOT_FILENAME%.cs
set STATE_DATA_CSHARP=%STATE_DATA_FILENAME%.cs

set OUTPUT_RELATIVE_DIR=..\robotConfiguration

call %ClassGenerator% --output=%OUTPUT_RELATIVE_DIR% --pascal- %ROBOT_SCHEMA%
call %ClassGenerator% --output=%OUTPUT_RELATIVE_DIR% --pascal- %STATE_DATA_SCHEMA%

call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%ROBOT_CSHARP% "[System.ComponentModel.DefaultValueAttribute(" "//[System.ComponentModel.DefaultValueAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%ROBOT_CSHARP% "[System.Xml.Serialization.XmlTypeAttribute(" "//[System.Xml.Serialization.XmlTypeAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%ROBOT_CSHARP% "[System.Xml.Serialization.XmlRootAttribute(" "//[System.Xml.Serialization.XmlRootAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%ROBOT_CSHARP% "[System.ComponentModel.DataAnnotations.RequiredAttribute()]" "//[System.ComponentModel.DataAnnotations.RequiredAttribute()]"

call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%STATE_DATA_CSHARP% "[System.ComponentModel.DefaultValueAttribute(" "//[System.ComponentModel.DefaultValueAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%STATE_DATA_CSHARP% "[System.Xml.Serialization.XmlTypeAttribute(" "//[System.Xml.Serialization.XmlTypeAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%STATE_DATA_CSHARP% "[System.Xml.Serialization.XmlRootAttribute(" "//[System.Xml.Serialization.XmlRootAttribute("
call replaceTextInFile.exe %OUTPUT_RELATIVE_DIR%\%STATE_DATA_CSHARP% "[System.ComponentModel.DataAnnotations.RequiredAttribute()]" "//[System.ComponentModel.DataAnnotations.RequiredAttribute()]"

