# cm12-i9300-UNOFFICIAL-downloader
A library and CLI front-end to download the latest nightly from http://forum.xda-developers.com/galaxy-s3/development/wip-cyanogenmod-12-t2936990

Usage:
cm12.i9300.UNOFFICIAL.downloader <path to folder>
eg
cm12.i9300.UNOFFICIAL.downloader C:\where\I\Like\To\Keep\Roms


What it will do:
1) try to find the latest nightly zip download link at the above forum
2) follow that link to the download page
3) grab the md5sum for the zip
4) attempt to download the zip (can resume if interrupted)
    - you even get live download progress (% complete, speed, eta)
5) verify that what it downloaded matches the md5sum on the download page
    - if not, it deletes the output and shows an error


Personally, I have this in a scheduled task that runs at 6 am on my pc so
I can just browse to my pc with FX File Explorer and copy the zip to my
phone to flash at my convenience.
