@ECHO OFF

set nunit_home=%1
set nunit_cli=%nunit_home%\bin\nunit-console.exe

echo nunit_home=%nunit_home%

set test1=Tests\Unit\DotNetUtilsUnitTests\bin\Debug\DotNetUtilsUnitTests.dll
set test2=Tests\Unit\UpdaterTests\bin\Debug\UpdaterTests.dll
set test3=Tests\Unit\IsanPluginTests\bin\Debug\IsanPluginTests.dll

%nunit_cli% "%test1%" /xml=nunit-result-1.xml
%nunit_cli% "%test2%" /xml=nunit-result-2.xml
%nunit_cli% "%test3%" /xml=nunit-result-3.xml
