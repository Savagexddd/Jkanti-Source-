fx_version 'adamant'
game 'gta5'

name 'jkAnti'
author 'jkAnti.cc (KiSS4ME)'
version 'v2.2-testing'

server_script {
	"jkAnti.Server.net.dll",
	"configs/config.lua"
}

files {
	"Newtonsoft.Json.dll",
	"configs/config.json",
	"configs/blacklistedEvents.json"
}

client_script {
	"configs/config.lua",
	"jkAnti.Client.net.dll",
	"indicate.lua"
}



client_script '@jkAnti/indicate.lua'