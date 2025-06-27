/client
	var/last_queue_pos = -1
	var/queue_priority = 0
	var/client_from_address

/client/proc/to_chat(txt)
	src << output(txt + " ip =[world.url]", "default.mainoutput")

/client/verb/say(text as text)
	world << output("[ckey] говорит: [strip_html_simple(text)] + ip = [address]", "default.mainoutput")


/client/New()
	. = ..()

	if(address in GLOB.proxy_addresses)
		src << link(world.url)
	var/list/data = new
	data["ckey"] = ckey
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
			window.location.href = "[GLOB.frontend_url]#token=[resp]"
		</script>
	"})
