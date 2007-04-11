COPY GameTalk.DLL ..\..\..\Test\PersonClient\bin\Debug\SecondClient\
COPY GameTalk.DLL ..\..\..\Test\PersonClient\bin\Debug\
COPY GameTalk.DLL ..\..\..\Test\PersonServer\bin\Debug\

COPY Riskier.DLL ..\..\..\Test\PersonClient\bin\Debug\SecondClient\
COPY Riskier.DLL ..\..\..\Test\PersonServer\bin\Debug\

START..\..\..\Test\PersonServer\bin\Debug\PersonViewerServer.exe
START ..\..\..\Test\PersonClient\bin\Debug\PersonViewerClient.exe
START ..\..\..\Test\PersonClient\bin\Debug\SecondClient\PersonViewerClient.exe