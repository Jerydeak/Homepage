@echo off

env\PageBuilder\PageBuilder\bin\Debug\net6.0\PageBuilder.exe -s blogs -d html\blogs\content
git add --all
git commit -m "Build Blogs"

pause