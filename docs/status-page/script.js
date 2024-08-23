
function setCookie(name, value, days) {
    const d = new Date();
    d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000));
    const expires = "expires=" + d.toUTCString();
    document.cookie = name + "=" + value + ";" + expires + ";path=/";
    document.cookie = name + "_expiry=" + d.toUTCString() + ";path=/";
}

function getCookie(name) {
    const cname = name + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(cname) == 0) {

            return c.substring(cname.length, c.length);
        }
    }
    return "";
}
function updateIDMessage() {
    
    const urlParams = new URLSearchParams(window.location.search);

    const uid = urlParams.get('uid') ? urlParams.get('uid').toUpperCase() : '';

    document.getElementById('unique-ID').textContent = uid;
}

function getCookieExpiry(cookieName) {
    const name = cookieName + "_expiry=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookieArray = decodedCookie.split(';');

    for (let i = 0; i < cookieArray.length; i++) {
        let cookie = cookieArray[i];
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(name) === 0) {
            return cookie.substring(name.length, cookie.length);
        }
    }
    return null;
}

function checkStepCompletion() {
    
    if (getCookie("step1Complete") === "true") {
        
        document.getElementById('step1Complete').checked = true;
        document.getElementById('preSurveyText').disabled = true;
        document.getElementById('submitPreSurveyText').disabled = true;
        document.getElementById('preSurveyLink').classList.add('disabled');
        document.getElementById('nextStepLink').classList.remove('disabled');
        document.getElementById('step1Title').innerHTML = "Step One Complete"
        
        var expiryDate = getCookieExpiry("step1Complete");

        document.getElementById('step2Description').innerHTML = 'You have until ' + expiryDate + ' to finish 10 sessions. Once you finish playing at least 10 sessions and complete the post survey, head back to Prolific to claim your reward.';
        
    }
    if (getCookie("step2Complete") === "true") {
        document.getElementById('step2Complete').checked = true;
        document.getElementById('step2Title').innerHTML = "Step Two Complete"

    }

    if (getCookie("step3Complete") === "true") {
        console.log("Checking the problem", document.getElementById('specialTextField'))
        document.getElementById('step3Complete').checked = true;
        document.getElementById('specialTextField').disabled = true;
        document.getElementById('submitPostSurveyText').disabled = true;
        document.getElementById('specialLink').classList.add('disabled');
        document.getElementById('step3Title').innerHTML = "Step Three Complete"
        var expiryDate = getCookieExpiry("step3Complete");
        document.getElementById('step2Description').innerHTML = 'Thank you for your participation! Head back to Prolific to claim your reward.';
        document.getElementById('nextStepLink').classList.add('disabled');
        document.getElementById('uidCountContainer').disabled = true;
        document.getElementById('prolific').classList.remove('disabled');
    }

}

document.addEventListener("DOMContentLoaded", function () {
    // TRACK STEPS COMPLETION
    checkStepCompletion();
    
    // SHOW UNIQUE ID ON TOP
    updateIDMessage();

    // EXTRACT UID FROM URL
    const uid = getUIDFromURL();

    // FUNCTION TO EXTRACT UID FROM URL
    function getUIDFromURL() {

        const urlParams = new URLSearchParams(window.location.search);

        return urlParams.get('uid') ? urlParams.get('uid').toUpperCase() : '';
    }

    // UPDATE UID BUTTON EVERY 4 MINUTES
    if (uid) {
        updateUidCountButton(uid);
        // Handle UID count logic
        setInterval(() => {
            // console.log("Updating UI every 4 minutes");
            updateUidCountButton(uid);
        }, 240000); // UPDATE EVERY 4 MINUTES

    }
    // UPDATE THE UID COUNT BUTTON
    function updateUidCountButton(uid) {
        const parser = new PublicGoogleSheetsParser('1-ZbloFuaSxy8jxOQQlK3O6scjx5R4C6rc7tYs_ZYBKM');
        parser.parse().then(data => {

            const container = document.getElementById('uidCountContainer');

            // Clear existing button
            container.innerHTML = '';
      
            const filteredData = data.filter(item => {
                return Object.keys(item).some(key => key.trim() === 'UID' && item[key] == uid);
            });
            
            const uidCount = filteredData.length;
            console.log(uidCount)

            const specialTextField = document.getElementById('specialTextField');

            // uidCountButton.className = 'uid-button';

            container.textContent = `You have completed ${uidCount} race sessions.`;

            const specialLink = document.getElementById('specialLink');

            //container.appendChild(uidCountButton);

            if (uidCount >= 10 & (getCookie("step3Complete") !== "true")) {
                // MARK STEP 2 AS COMPLETE
                document.getElementById('step2Complete').checked = true;

                // ENABLE STEP 3 LINK
                specialLink.classList.remove('disabled');
                specialLink.removeAttribute('disabled');
                //console.log("Is this the reason this is happening?")
                // ENABLE POST SURVEY TEXT
                specialTextField.classList.remove('disabled');
                specialTextField.removeAttribute('disabled');

                // ENABLE POST SURVEY BUTTON
                submitPostSurveyText.disabled = false;

                document.getElementById('step2Title').innerHTML = "Step Two Complete";
                

                setCookie("step2Complete", "true", 30);
            }
        }).catch(error => {
            console.error('Error fetching data:', error);
            if (error.message.includes('Quota exceeded')) {
                setTimeout(() => {
                    updateUidCountButton(uid);
                }, 20000); // Retry after 20 seconds
            }
        });
    }

    // HANDLE PRE-SURVEY FORM SUBMISSION
    const preSurveyText = document.getElementById('preSurveyText');
    const submitPreSurveyText = document.getElementById('submitPreSurveyText');
    const specialTextField = document.getElementById('specialTextField');
    const submitPostSurveyText = document.getElementById('submitPostSurveyText');
    const preSurveyLink = document.getElementById('preSurveyLink');
    const nextStepLink = document.getElementById('nextStepLink');


    submitPreSurveyText.addEventListener('click', function (event) {
        event.preventDefault(); // Prevent default form submission

        // Check if preSurveyText is filled and has the correct value
        if (!preSurveyText.value.trim() || preSurveyText.value.trim() !== "RACEGAME") {
            alert('Please enter the correct value that you found from the PRE survey page.');
            preSurveyText.style.border = '6px solid red';
            preSurveyText.value = "";
        } else {
            // RESET BORDER
            preSurveyText.style.border = '';
            
            // CLEAR THE VALUE IN THE TEXTFIELD
            preSurveyText.value = ""
            
            // DISABLE PRE SURVEY TEXTFIELD
            preSurveyText.disabled = true;
            
            // DISABLE PRE SURVEY BUTTON
            submitPreSurveyText.disabled = true;
            
            // DISABLE PRE SURVEY LINK
            preSurveyLink.classList.add('disabled');

            // MARK STEP 1 AS COMPLETE
            document.getElementById('step1Complete').checked = true;
            
            document.getElementById('step1Title').innerHTML = "Step One Complete"

            // ENABLE STEP 2 LINK
            nextStepLink.classList.remove('disabled');

            // SET COOKIE
            setCookie("step1Complete", "true", 30);

            // CHANGE HEADEAR TEXT
	        var date = new Date();
            date.setTime(date.getTime() + (30 * 24 * 60 * 60 * 1000));
            var expiryDate = date.toUTCString();

            // UPDATE HEADER TEXT

            document.getElementById('step2Description').innerHTML = 'You have until ' + expiryDate + ' to finish 10 or more race sessions.';

        }

        
    });

    // POST SURVEY
    submitPostSurveyText.addEventListener('click', function (event) {
        event.preventDefault(); // Prevent default form submission

        // Check if specialTextField has the correct value
        if (!specialTextField.value.trim() || specialTextField.value.trim() !== "RACESCOMPLETE") {
            alert('Please enter the correct value that you found from the POST survey page.');
            specialTextField.style.border = '6px solid red';
            specialTextField.value = "";
        } else {
            // MARK STEP 3 AS COMPLETE
            document.getElementById('step3Complete').checked = true;
            document.getElementById('step1Title').innerHTML = "Step 3 Complete"

            specialTextField.style.border = ''; // Reset border

            // CLEAR THE POST SURVEY VALUE
            specialTextField.value = "";

            // DISABLE POST SURVEY TEXTFIELD
            specialTextField.disabled = true;
            // DISABLE POST SURVEY BUTTON
            submitPostSurveyText.disabled = true;

            // DISABLE POST SURVEY LINK
            document.getElementById('specialLink').classList.add('disabled');

            // DISABLE STEP 2 LINK

            document.getElementById('nextStepLink').classList.add('disabled');

            // THANK PARTICIPANTS FOR THEIR PARTICIPATION
            document.getElementById('step2Description').innerHTML = 'Thank you for your participation! Head back to Prolific for your reward.';

            // DISABLE RACE SESSION COUNT BUTTON

            document.getElementById('uidCountContainer').disabled = true;

            // ENABLE PROLIFIC LINK

            document.getElementById('prolific').classList.remove('disabled');

            // SET STEP 3 COOKIE

            setCookie("step3Complete", "true", 30);
        }
    });
});
document.addEventListener("DOMContentLoaded", function() {
    const questions = document.querySelectorAll('.q');
    const answers = document.querySelectorAll('.a');
    const arrows = document.querySelectorAll('.arrow');

    for (let i = 0; i < questions.length; i++) {
        questions[i].addEventListener('click', () => {
            answers[i].classList.toggle('a-opened');
            arrows[i].classList.toggle('arrow-rotated');
        });
    }
});
