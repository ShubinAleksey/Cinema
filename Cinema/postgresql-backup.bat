@echo off
%1\bin\pg_dump.exe -f %2 -E UTF8 -d %3
exit