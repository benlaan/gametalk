@ECHO OFF

ECHO ------------ CLIENT 1 --------------
D:
CD .\DEBUG\
START RiskClient.EXE
CD ..

ECHO ------------ CLIENT 2 --------------
D:
CD .\DEBUG1\
START RiskClient.EXE
CD ..

ECHO ------------ CLIENT 3 --------------
D:
CD .\DEBUG2\
START RiskClient.EXE
CD ..

