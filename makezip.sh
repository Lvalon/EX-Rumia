#!/bin/sh
cd /Users/e/Library/Application\ Support/Steam/steamapps/common/LBoL/BepInEx/scripts
mkdir lvalonexrumia
rm -r lvalonexrumia/
cp -R -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/DIRRESOURCES/. lvalonexrumia/
cp -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/bin/Debug/netstandard2.1/lvalonexrumia.dll lvalonexrumia/
cp -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/CHANGELOG.md lvalonexrumia/
cp -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/CREDITS.md lvalonexrumia/
cp -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/icon.png lvalonexrumia/
cp -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/manifest.json lvalonexrumia/
cp -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/README.md lvalonexrumia/
cp -a /Users/e/Desktop/tachyon\ transmigration/projects/indev/lvalonexrumia/modinfo.json lvalonexrumia/
rm -fr lvalonexrumia/Thumbs.db
zip -r -j lvalonexrumia.zip lvalonexrumia/*
