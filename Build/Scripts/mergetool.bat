@ECHO OFF

set SolutionDir=%1
set TargetFileName=%2
set TargetPath=%3
set TargetDir=%4

%SolutionDir%Build\Tools\ILMerge\ILMerge.exe /v4 /ndebug /target:exe /wildcards /out:%SolutionDir%Build\Tools\%TargetFileName% %TargetPath% %TargetDir%*.dll
