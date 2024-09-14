
#define PROC_REF(X) (nameof(.proc/##X))

#define CALL_EXT call_ext
// Formats a queue text message
#define QUEUETEXT(message) "<font color='#FFAA00'>\[Queue] [message]</font>"

// Formats an info text message
#define INFOTEXT(message) "<font color='#FFFFFF'>\[Info] [message]</font>"

#define MAX_MESSAGE_LEN 300


#define GLOBAL_PROC	"some_magic_bullshit"

#define CALLBACK new /datum/callback
#define INVOKE_ASYNC ImmediateInvokeAsync

/// like CALLBACK but specifically for verb callbacks
#define VERB_CALLBACK new /datum/callback/verb_callback
