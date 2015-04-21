xda-frequently-updated-rom-downloader

A library and CLI front-end to download the latest rom from the XDA thread of your choice

Usage:
xda-rom-downloader -p {project id} -o <path to folder>
eg
xda-rom-downloader -p 8923 -o C:\where\I\Like\To\Keep\Roms


What it will do:
1) try to find the latest nightly zip download link at the download section for the
    project you specify by id
2) follow that link to the download page
3) grab the md5sum for the zip
4) attempt to download the zip (can resume if interrupted)
    - you even get live download progress (% complete, speed, eta)
5) verify that what it downloaded matches the md5sum on the download page
    - if not, it deletes the output and shows an error


Personally, I have this in a scheduled task that runs at 6 am on my pc so
I can just browse to my pc with FX File Explorer and copy the zip to my
phone to flash at my convenience. I use this to keep up to date with the unofficial
i9300 cm12.1 rom.
