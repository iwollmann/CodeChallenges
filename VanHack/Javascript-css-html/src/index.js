mapboxgl.accessToken = "ADD_ACCESS_TOKEN";

const Configs = {
    camera: {
        center: [-123.1939438, 49.2578263],
        zoom: 9.68,
        pitch: 0,
        borough: 0
    },
    localStorageKey: "@vanhack:events#applications",
    tweetContentTemplate: (event) => `Take a look at @VanHack '${event.title}' on ${event.date}`
};

const map = new mapboxgl.Map({
    container: "map",
    style: "mapbox://styles/mapbox/satellite-v9",
    center: Configs.camera.center,
    maxZoom: 16,
    minZoom: 9,
    zoom: Configs.camera.zoom,
    scrollZoom: false,
    dragPan: false,
    dragRotate: false,
    doubleClickZoom: false,
});

let currentMarker;

let events = [];

function loadEvents() {
    // ##### IMPORTANT #######
    // The events url cannot have the "#" because of the linked in redirect
    return [
        {
            id: 0,
            title: "Hire international talent at VanHack Leap Vancouver with VEC.",
            title_short: "VanHack Leap Vancouver with VEC",
            url: "https://vanhack.com/platform/events/vanhack-leap-vancouver-with-vec",
            image: "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSoigEmiTSk_Zfo1uwwRkGl-kwQ13ITlWsgtU3gKhyP1IXGdXcr&usqp=CAU",
            description:
                "Is your BC tech company looking to hire? Need the best senior software engineers and developers from across the globe? OK, then. You’re invited to our next Leap event, hosted by Vancouver Economic Commission! What can you expect at this Leap Vancouver event? Leap is a 3-day event that kicks off with a fun networking …",
            date: "September 23, 2020",
            type: "leap",
            camera: {
                center: [-123.1170321, 49.2812002],
                zoom: 15.21,
                pitch: 90,
                borough: -90,
            },
        },
        {
            id: 1,
            title: "Webinar at new apple offices",
            url: "https://vanhack.com/platform/events/new-apple-offices-webinar",
            description:
                "At apple, we believe that technology is most powerful when it empowers everyone. Join us on our new office to learn how you can take advantage of the award-winning accessibility features that come standard on Apple devices.",
            image: "https://cdn.mobilesyrup.com/wp-content/uploads/2019/07/apple-office-vancouver.jpg",
            date: "July 02, 2020",
            type: "premium",
            camera: {
                center: [-123.1179656, 49.2806156],
                bearing: -8.9,
                zoom: 14.68,
                pitch: 30,
                borough: -30,
            },
        },
        {
            id: 2,
            title: "Vancouver Recruiting Mission",
            url: "https://vanhack.com/platform/events/vancouver-recruiting-mission",
            description:
                "Interview Vetted Senior Developers ready to relocate and work for you during a weekend long hackathon and recruiting fair",
            type: "mission",
            date: "March 03, 2020",
            image: "https://assets.simpleviewinc.com/simpleview/image/upload/c_fill,h_808,q_75,w_1440/v1/clients/vancouverbc/VCC_0c3f4b2e-9dc9-4716-9581-c48a26173be4.jpg",
            camera: {
                center: [-123.1190796, 49.2891193],
                bearing: 25.3,
                zoom: 16.5,
                pitch: 50,
                borough: -50,
            },
        },
        {
            id: 3,
            title: "How to work at Amazon",
            url: "https://vanhack.com/platform/events/how-to-work-at-amazon-webinar",
            description:
                "This webinar explains how to enter Amazon Vancouver! They are hiring more than 500 people next year, will you be one of them? See the vacancies here.",
            type: "webinar",
            image: "https://images.dailyhive.com/20180430095420/post-office-amazon.jpg",
            date: "December 04, 2020",
            camera: {
                center: [-123.1191908, 49.2809928],
                bearing: 36,
                zoom: 14.37,
                pitch: 90,
                borough: -90,
            },
        },
        {
            id: 4,
            title: "Vancouver Meetup at Microsoft",
            url: "https://vanhack.com/platform/events/vancouver-meetup-at-microsoft",
            description:
                "Join us if you are developers who want to learn more about React and/or is looking for a job, recruiters who want to hire React developers, or entrepreneurs who wish to meet new people.",
            image: "https://www.straight.com/files/v3/styles/gs_standard/public/images/16/02/microsoft-from_mic_cda.jpg?itok=xAHa4ZHG",
            type: "meetup",
            date: "September 20, 2020",
            camera: {
                center: [-123.1169764, 49.2768626],
                bearing: 28.4,
                zoom: 13.64,
                pitch: 70,
                borough: -70,
            },
        },
        {
            id: 5,
            title: "COVID-19 VanHackathon",
            url: "https://vanhack.com/platform/events/covid-19-vanhackathon",
            description:
                `With the World Health Organization (WHO) declaring coronavirus (COVID-19) a global pandemic, governments have issued guidance for members of the community to practice social distancing, while companies have enforced work from home policies in an effort to flatten the curve of viral infections across the population. Most schools are closed and young people are stuck indoors so why not invest some of your free time into making a difference to the world.`,
            type: "vanhackathon",
            image: "https://www.cambridgecrossing.com/wp-content/uploads/2018/12/G4.jpg",
            date: "September 18, 2020",
            camera: {
                center: [-123.112271, 49.2844198],
                zoom: 14.68,
                pitch: 50,
                borough: -50,
            },
        },
    ];
};

function goTo(cameraOptions) {
    map.flyTo({ ...cameraOptions, essential: true });
}

function applyToEvent(el, id) {
    const applyments = JSON.parse(localStorage.getItem(Configs.localStorageKey) || '[]');

    localStorage.setItem(Configs.localStorageKey, JSON.stringify([...applyments, id]));

    const successMsg = document.createElement("div");
    successMsg.classList.add('registered');

    const label = document.createElement('span');
    label.textContent = "Your are registered for this.";

    successMsg.appendChild(label);
    el.parentElement.appendChild(successMsg);
    el.remove();
}

function resetCamera() {
    goTo(Configs.camera);
}

function resetMarker() {
    if (currentMarker) {
        currentMarker.remove();
    }
}

function resetMenu() {
    var menu = document.querySelector(".menu-event-container");
    if (menu) {
        menu.classList.toggle('disappear');

        const animationEndCallback = (e) => {
            menu.removeEventListener('animationend', animationEndCallback);
            menu.remove();
            renderMenuEvents()
        }

        menu.addEventListener('animationend', animationEndCallback);
    } else {
        renderMenuEvents();
    }
}

function navigateBack() {
    resetCamera();
    resetMarker();
    resetMenu();
}

function renderMenuEvent(id) {
    var menu = document.createElement('div');
    menu.setAttribute('class', 'menu-event-container');
    menu.innerHTML = templateEventDetails(events[id]);
    document.body.appendChild(menu);
}

function templateApplyButton(id) {
    const applyments = JSON.parse(localStorage.getItem(Configs.localStorageKey) || '[]');

    if (applyments && applyments.includes(id)) {
        return `<div class="registered"><span>You are already registered!</span></div>`
    }

    return `<button onclick="applyToEvent(this, ${id})">Apply</button>`;
}

function templateEventDetails(item) {
    return `<div class="${item.type || ''}">
      <div class="menu-event-back-link" onclick="navigateBack();">
        <i class="fa fa-chevron-right"> </i>
      </div>
      <img
        src=${item.image}
      />
      <div class="menu-event-content">
        <div>
            <h2>${item.title}</h2>
            <div class="social-share">
            <a
                href="https://www.linkedin.com/sharing/share-offsite/?url=${item.url}"
                target="_blank"
            >
                <i class="fab fa-linkedin fa-2x"></i>
            </a>
            <a
                href="https://twitter.com/intent/tweet?text=${Configs.tweetContentTemplate(item)}"
                target="_blank"
            >
                <i class="fab fa-twitter fa-2x"></i>
            </a>
            </div>
        </div>
        <span class="menu-event-schedule">
          <i class="fa fa-calendar"></i>
          ${item.date}
        </span>
        <p>
            ${item.description}
        </p>
        <a href="${item.url}" target="_blank">see more...</a>
      </div>
      ${item.type === "premium" ? `<button
            class="premium"
            onclick="window.open('https://vanhack.com/premium')"
            >
            Join Vanhack Premium
        </button>`: templateApplyButton(item.id)}  
    </div>`;
}

function renderMenuEvents() {
    let htmlMenuEvents = '';
    for (let index = 0; index < events.length; index++) {
        htmlMenuEvents += templateMenuItem(events[index]);
    }

    let menu = document.querySelector(".menu-events-container");
    if (!menu) {
        menu = document.createElement('div');
        menu.setAttribute('class', 'menu-events-container');
        document.body.appendChild(menu);
    }

    menu.innerHTML = htmlMenuEvents;
};

function handleOnclickMenuItem(id) {
    const event = events[id];

    goTo(event.camera);

    currentMarker = new mapboxgl.Marker()
        .setLngLat(event.camera.center)
        .addTo(map);

    var menu = document.querySelector(".menu-events-container");
    menu.classList.toggle('disappear');

    const animationEndCallback = (e) => {
        menu.removeEventListener('animationend', animationEndCallback);
        menu.remove();
        renderMenuEvent(id);
    }

    menu.addEventListener('animationend', animationEndCallback);
}

function templateMenuItem(item) {
    return `<div class="event-card ${item.type || ''}" onclick="handleOnclickMenuItem(${item.id})">
        <div>
            <span class=${item.type}-tag>${item.type}</span>
            <h2 id="location-title">${item.title_short || item.title}</h2>
        </div>
        <p id="location-description">${item.description}</p>
        <div class="details-label"><span>See Details <i class="fa fa-angle-right"></i></span></div>
    </div>`;
};

map.on("load", function () {
    events = loadEvents();
    renderMenuEvents();
});