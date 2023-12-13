/client
	var/last_queue_pos = -1
	var/queue_priority = 0
	var/client_from_address

/client/proc/to_chat(txt)
	src << output(txt, "default.mainoutput")

/client/verb/say(text as text)
	world << output("[ckey] говорит: [strip_html_simple(text)]", "default.mainoutput")

/client/Topic(href, href_list, hsrc)
	var/log_topic = "client [ckey] with BYOND [byond_version].[byond_build] TopicDat = [href]"
	world.log << log_topic
	to_chat(INFOTEXT(log_topic))
	to_chat(INFOTEXT("Подключаемся..."))
	if(!href_list["target"])
		to_chat(INFOTEXT("У вас пустой топик"))
		client_from_address = "пустой топик"
	else
		client_from_address = href_list["target"]

	if(!(client_from_address in GLOB.supported_servers))
		to_chat(INFOTEXT("Вы пришли не с нашего сервера([client_from_address])"))
		sleep(40)
		del(src)
		return
	to_chat(INFOTEXT("Вы пытаетесь подключиться к серверу - [client_from_address]"))

	// If we dont need to queue, go straight to allowing entry
	if(IgnoreQueue())
		AllowEntry()
		return

	// Otherwise, put them in the queue list
	GLOB.server_queues[client_from_address] += src

/client/

// Cleanup
/client/Del()
	var/list/queue = GLOB.server_queues[client_from_address]
	if(queue)
		queue.Remove(src)

/client/proc/IgnoreQueue()
	to_chat(QUEUETEXT("Проверяем не освободилось ли место</b>"))
	try
		var/result = world.Export("byond://[client_from_address]?queue_status_220&ckey_check=[ckey]&key=[GLOB.gameserver_commskey]", null, TRUE)
		var/list/result_data = json_decode(result)
		to_chat("<font color='lime'>\[Info][result_data["reason"]].</font>")
		queue_priority = result_data["donator_level"]
		return result_data["allow_player"]
	catch
		to_chat("<font color='lime'>\[Info]К сожалению сервер не работает, можете подождать в очереди но лучше спросите в дискорде проекта</font>")
		return FALSE

