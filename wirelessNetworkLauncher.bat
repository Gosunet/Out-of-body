netsh wlan stop hostednetwork

netsh wlan set hostednetwork mode=allow ssid="outofbody" key=outofbody
netsh wlan start hostednetwork

pause