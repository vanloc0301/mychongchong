for %%a in ("%cd%\*.resources") do (
      echo ResGen.exe  %%a  %%a.ok>> run.bat
  )
