// Holds all the global vars
/datum/global_var_holder
	var/supported_servers = list()
	// The server commskey
	var/gameserver_commskey
	// List of clients in the queue
	var/list/server_queues = list()
	var/list/proxy_addresses = list()
	var/http_log = "data/logs/http.log"
	var/log_end = ""
	var/datum/http_system/SShttp
	var/frontend_url = ""
	var/backend_url = ""

var/datum/global_var_holder/GLOB = new() // Initialise it so we can grab it with GLOB.

//Simply removes < and > and limits the length of the message
/proc/strip_html_simple(t, limit=MAX_MESSAGE_LEN)
	var/list/strip_chars = list("<",">")
	t = copytext(t,1,limit)
	for(var/char in strip_chars)
		var/index = findtext(t, char)
		while(index)
			t = copytext(t, 1, index) + copytext(t, index+1)
			index = findtext(t, char)
	return t
