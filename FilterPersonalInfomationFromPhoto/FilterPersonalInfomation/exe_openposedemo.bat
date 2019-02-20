@echo off

setlocal

cd %1
bin\OpenPoseDemo.exe --hand --image_dir %2 --write_json %3 --display 0 --net_resolution "320x240" --render_pose 0

endlocal
