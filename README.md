### 302CodeGenerator

The 302CodeGenerator is a .NET application which uses the robot definition xml files used by Team 302 in order to generate some of the C++ robot files.

#Why auto generate code?
All the information required to write the corresponding C++ classes is available in the xml files. So the correct question is >Why would you want to write the code by hand?

#Inputs

-robot.xml -> the main robot definition file
-_mechanism_.xml -> the states definition of a mechanism. The name of the file should be the name of the mechanism and is used in code generation
