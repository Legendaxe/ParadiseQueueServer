// Inside here are all the procs you NEED to override if you wish to use this implementation in production

// Override this to check whether the player connecting is a new player or someonme who has played before
// However you do this will depend on your codebase
/client/proc/NewPlayer()
	return TRUE

// Override this to do whatever you want to do to route your client from this server to your main one
/client/proc/AllowEntry()
	try
		to_chat("<font color='lime'>\[Info] Пробуем выдать пропуск для сикея [ckey] на сервер</font>")
		var/result = world.Export("byond://[client_from_address]?queue_bypass&ckey_check=[ckey]&key=[GLOB.gameserver_commskey]", null, TRUE)
		var/list/result_data = json_decode(result)
		to_chat("Запрос отправлен")

		if(result_data["player_added"])
			to_chat("<font color='lime'>\[Info] Доступ выдан, вас перенаправляет на сервер</font>")
			sleep(20)

			src << link("byond://[client_from_address]")
			sleep(20)
			del(src)
			return
	catch
		to_chat("<font color='lime'>\[Info]Сервер не смог ответить, подождите еще немного или спросите в дискорде</font>")

