/proc/auxtools_stack_trace(msg)
	CRASH(msg)

/proc/enable_debugging(mode, port)
	CRASH("auxtools not loaded")

/proc/auxtools_expr_stub()
	CRASH("auxtools not loaded")

// Called in world/Del(). This is VERY important, otherwise you get phantom threads which try to lookup RAM they arent allowed to
/world/proc/disable_auxtools_debugger()
	var/debug_server = world.GetConfig("env", "AUXTOOLS_DEBUG_DLL")
	if(debug_server)
		CALL_EXT(debug_server, "auxtools_shutdown")()


/world/New()
	var/debug_server = world.GetConfig("env", "AUXTOOLS_DEBUG_DLL")
	if(debug_server)
		CALL_EXT(debug_server, "auxtools_init")()
		enable_debugging()
	. = ..()
	world.log << "Loading..."
	GLOB.SShttp = new
	GLOB.SShttp.Initialize()
	load_configuration()
	start_tickloop()
	world.log << "Load Complete"

/world/Del()
	disable_auxtools_debugger()
	rustg_close_async_http_client()
	. = ..()


/proc/load_configuration()
	if(!fexists("config.json"))
		world.log << "\[ERROR] config.json does not exist. World will exit."
		del(world)
		return

	var/list/config = json_decode(file2text("config.json"))
	for(var/server in config["supported_servers"])
		GLOB.supported_servers += server
		GLOB.server_queues += list("[server]" = list())
	for(var/ip in config["proxy_addresses"])
		GLOB.proxy_addresses += ip
	GLOB.gameserver_commskey = config["gameserver_commskey"]
	GLOB.frontend_url = config["frontend_url"]
	GLOB.backend_url = config["backend_url"]


/proc/start_tickloop()
	// This has a selfcontained loop. Do not wait.
	set waitfor = FALSE
	while(TRUE)
		GLOB.SShttp.fire()
		sleep(5)
