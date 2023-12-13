/world/New()
	world.log << "Loading..."
	load_configuration()
	start_tickloop()
	world.log << "Load Complete"


/proc/load_configuration()
	if(!fexists("config.json"))
		world.log << "\[ERROR] config.json does not exist. World will exit."
		del(world)
		return

	var/list/config = json_decode(file2text("config.json"))
	for(var/server in config["supported_servers"])
		GLOB.supported_servers += server
		GLOB.server_queues += list("[server]" = list())
	GLOB.gameserver_commskey = config["gameserver_commskey"]

/proc/start_tickloop()
	// This has a selfcontained loop. Do not wait.
	set waitfor = FALSE

	while(TRUE)
		for(var/address in GLOB.server_queues)

			var/list/queue = GLOB.server_queues[address]

			for(var/client/client in queue)
				var/client_position = queue.Find(client)
				if(client_position != client.last_queue_pos)
					client.to_chat(QUEUETEXT("Сейчас вы <b>[client_position]-ый</b> из <b>[length(queue)] клиентов</b>"))
					client.last_queue_pos = client_position

				if(client.IgnoreQueue())
					client.AllowEntry()
				sleep(3)

		sleep(100)
