@ECHO OFF

CALL UPDATE.BAT

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
