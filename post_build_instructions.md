#Build instructions
1. In Unity, make sure compression on the build is disabled for a WebGL build
1. Increment the build number if necessary
1. Create a WebGL build in a new folder in docs/, for instance, docs/PCBuild1.3.0/

#Deploy instructions
1. Before committing the build to git, add these few lines to index.html in the build folder (to discourage people from reloading the page):
	1. Just below the <body> tag on a new line, add this: <div style="height: 100px"><h1 style="background: gray; width: 960px; text-align: center; margin: auto"><b>Please avoid refreshing this page while a race is active</b></h1></div>
	1. At the bottom of the page just before the </script> tag, add this: window.onbeforeunload = function () {return false;}
	1. Add this just after the "unity-warning" div: <div><p style="text-align: center">If you need help or have questions, contact: <a href="mailto:cubeslab.memphis@gmail.com">cubeslab.memphis@gmail.com</a></p></div>
1. Commit the build in the docs subfolder to the main branch and push to GitHub. This will automatically be accessible from GitHub pages
