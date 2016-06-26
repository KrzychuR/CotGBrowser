
function CotGBrowser_RegisterChatListeners(t)
{
    //t - musi byc jakis argument
    $('#chatDisplay').bind('DOMNodeInserted', function (e) {
        hobbita.world_chat_message(e.target.innerText, e.target.innerHTML);
    });
}
