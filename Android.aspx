<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Android.aspx.cs" Inherits="TheDealPortal.Android" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="Scripts/jplayer.blue.monday.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>
<script type="text/javascript" src="Scripts/jquery.jplayer.min.js"></script>
<script type="text/javascript" src="Scripts/jplayer.playlist.min.js"></script>
<script type="text/javascript">
//<![CDATA[
    $(document).ready(function playFlashVideo(mySrc) {
 
      
        var FileName1 = $('#HiddenField1').val();
        var Role1 = $('#HiddenField2').val();
        var FileName2 = $('#HiddenField3').val();
        var Role2 = $('#HiddenField4').val();
        var FileName3 = $('#HiddenField5').val();
        var Role3 = $('#HiddenField6').val();
        var FileName4 = $('#HiddenField7').val();
        var Role4 = $('#HiddenField8').val();
        var FileName5 = $('#HiddenField9').val();
        var Role5 = $('#HiddenField10').val();
        var FileName6 = $('#HiddenField11').val();
        var Role6 = $('#HiddenField12').val();
        var FileName7 = $('#HiddenField13').val();
        var Role7 = $('#HiddenField14').val();
        var FileName8 = $('#HiddenField15').val();
        var Role8 = $('#HiddenField16').val();
        var FileName9 = $('#HiddenField17').val();
        var Role9 = $('#HiddenField18').val();
        var FileName10 = $('#HiddenField19').val();
        var Role10 = $('#HiddenField20').val();

        new jPlayerPlaylist({
            jPlayer: "#jquery_jplayer_1",
            cssSelectorAncestor: "#jp_container_1"
        }, [
		{
		    title: FileName1,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role1 + "/" + FileName1,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role1 + "/" + FileName1
		},
        {
        
        title: FileName2,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role2 + "/" + FileName2,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role2 + "/" + FileName2
          
        },
        {
      //  if(FileName2!=null)
        title: FileName3,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role3 + "/" + FileName3,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role3 + "/" + FileName3
        },
        {
       // if(FileName2!=null)
        title: FileName4,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role4 + "/" + FileName4,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role4 + "/" + FileName4
        },
        {
        //if(FileName2!=null)
        title: FileName5,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role5 + "/" + FileName5,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role5 + "/" + FileName5
        },
        {
        //if(FileName2!=null)
        title: FileName6,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role6 + "/" + FileName6,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role6 + "/" + FileName6
        },
        {
       // if(FileName2!=null)
        title: FileName7,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role7 + "/" + FileName7,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role7 + "/" + FileName7
        },
        {
       // if(FileName2!=null)
        title: FileName8,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role8 + "/" + FileName8,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role8 + "/" + FileName8
        },
        {
        //if(FileName2!=null)
        title: FileName9,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role9 + "/" + FileName9,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role9 + "/" + FileName9
        },
        {
        //if(FileName2!=null)
        title: FileName10,
		    mp3: "http://local.thedeal1.info/local/TempFile/Recording/" + Role10 + "/" + FileName10,
		    oga: "http://local.thedeal1.info/local/TempFile/Recording/" + Role10 + "/" + FileName10
        }



	], {
	    swfPath: "js",
	    supplied: "oga, mp3",
	    wmode: "window",
	    smoothPlayBar: true,
	    keyEnabled: true
	});
    });
//]]>
</script>
    <title></title>
</head>
<body>
     
		<form id="form1" runat="server">
     
		<div id="jquery_jplayer_1" class="jp-jplayer">
            
        </div>

		<asp:HiddenField ID="HiddenField1" runat="server" />

		<asp:HiddenField ID="HiddenField3" runat="server" />

		<asp:HiddenField ID="HiddenField4" runat="server" />

		<asp:HiddenField ID="HiddenField5" runat="server" />

		<asp:HiddenField ID="HiddenField6" runat="server" />

		<asp:HiddenField ID="HiddenField7" runat="server" />

		<asp:HiddenField ID="HiddenField8" runat="server" />

		<asp:HiddenField ID="HiddenField9" runat="server" />

		<asp:HiddenField ID="HiddenField10" runat="server" />

		<asp:HiddenField ID="HiddenField11" runat="server" />

		<asp:HiddenField ID="HiddenField12" runat="server" />

		<asp:HiddenField ID="HiddenField13" runat="server" />

		<asp:HiddenField ID="HiddenField14" runat="server" />

		<asp:HiddenField ID="HiddenField15" runat="server" />

		<asp:HiddenField ID="HiddenField16" runat="server" />

		<asp:HiddenField ID="HiddenField17" runat="server" />

		<asp:HiddenField ID="HiddenField18" runat="server" />

		<asp:HiddenField ID="HiddenField19" runat="server" />

		<asp:HiddenField ID="HiddenField20" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
		<div id="jp_container_1" class="jp-audio">
			<div class="jp-type-playlist">
				<div class="jp-gui jp-interface">
					<ul class="jp-controls">
						<li><a href="javascript:;" class="jp-previous" tabindex="1">previous</a></li>
						<li><a href="javascript:;" class="jp-play" tabindex="1">play</a></li>
						<li><a href="javascript:;" class="jp-pause" tabindex="1">pause</a></li>
						<li><a href="javascript:;" class="jp-next" tabindex="1">next</a></li>
						<li><a href="javascript:;" class="jp-stop" tabindex="1">stop</a></li>
						<li><a href="javascript:;" class="jp-mute" tabindex="1" title="mute">mute</a></li>
						<li><a href="javascript:;" class="jp-unmute" tabindex="1" title="unmute">unmute</a></li>
						<li><a href="javascript:;" class="jp-volume-max" tabindex="1" title="max volume">max volume</a></li>
					</ul>
					<div class="jp-progress">
						<div class="jp-seek-bar">
							<div class="jp-play-bar"></div>
						</div>
					</div>
					<div class="jp-volume-bar">
						<div class="jp-volume-bar-value"></div>
					</div>
					<div class="jp-time-holder">
						<div class="jp-current-time"></div>
						<div class="jp-duration"></div>
					</div>
					<ul class="jp-toggles">
						<li><a href="javascript:;" class="jp-shuffle" tabindex="1" title="shuffle">shuffle</a></li>
						<li><a href="javascript:;" class="jp-shuffle-off" tabindex="1" title="shuffle off">shuffle off</a></li>
						<li><a href="javascript:;" class="jp-repeat" tabindex="1" title="repeat">repeat</a></li>
						<li><a href="javascript:;" class="jp-repeat-off" tabindex="1" title="repeat off">repeat off</a></li>
					</ul>
				</div>
				<div class="jp-playlist">
					<ul>
						<li></li>
					</ul>
				</div>
				<div class="jp-no-solution">
					<span>Update Required</span>
					To play the media you will need to either update your browser to a recent version or update your <a href="http://get.adobe.com/flashplayer/" target="_blank">Flash plugin</a>.
				</div>
			</div>
		</div>
     </form>
        </form>
</body>
</html>
