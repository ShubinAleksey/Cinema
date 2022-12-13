@echo off
%1\bin\psql.exe -d %2 -f %3
exit