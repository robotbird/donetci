
for /r %%d in (.) do if exist "%%d\debug" rd /s /q "%%d\debug"
for /r %%d in (.) do if exist "%%d\Release" rd /s /q "%%d\Release"