/client
	var/last_queue_pos = -1
	var/queue_priority = 0
	var/client_from_address

/client/proc/to_chat(txt)
	src << output(txt + " ip =[world.url]", "default.mainoutput")

/client/verb/say(text as text)
	world << output("[ckey] говорит: [strip_html_simple(text)] + ip = [address]", "default.mainoutput")

// Cleanup
/client/Del()
	var/list/queue = GLOB.server_queues[client_from_address]
	if(queue)
		queue.Remove(src)

/client/New()
	. = ..()

	var/list/data = new
	data["ckey"] = ckey
	if(address == "192.168.1.52")
		src << link(world.url)

	data["ip"] = address
	to_chat("[address ? address : "lol"] - ip")
	var/list/headers = new
	headers["Content-Type"] = "Application/Json"

	var/datum/callback/cb = CALLBACK(src, PROC_REF(on_client_authorize))
	world.log << "post: [json_encode(data)]"

	GLOB.SShttp.create_async_request(
	RUSTG_HTTP_METHOD_POST,
	"[GLOB.backend_url]/api/lobby-connect",
	json_encode(data),
	headers,
	cb
	)

/client/proc/on_client_authorize(datum/http_response/response)
	world.log << "Response [src] [response.body]"
	var/list/resp = json_decode(response.body)
	src << browse({"
		<script>
			window.location.href = "[GLOB.frontend_url]#token=[resp["token"]]"
		</script>
	"})
