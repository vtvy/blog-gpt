const sendQuestionBtn = document.querySelector("#questionInput + img");
const chatInput = document.querySelector("#questionInput");
const chatBox = document.querySelector(".chatbot ul");
const chatbotToggleBtn = document.querySelector(".chat-toggle-btn");

chatbotToggleBtn.addEventListener("click", () =>
	document.querySelector(".chatbot").classList.toggle("show-chat")
);

let userQuestion;

const createUserChatLine = (text) => {
	const chatLine = document.createElement("li");
	chatLine.classList.add("question");
	let chatContent = document.createElement("p");
	chatContent.classList.add(
		"bg-primary",
		"rounded-2",
		"border",
		"border-success-subtle"
	);
	chatContent.textContent = text;
	chatLine.appendChild(chatContent);
	return chatLine;
};

const createAnswerChatLine = (text) => {
	const chatLine = document.createElement("li");
	chatLine.classList.add("answer");
	let chatContent = document.createElement("p");
	chatContent.classList.add(
		"bg-warning",
		"rounded-2",
		"border",
		"border-primary-subtle"
	);
	chatContent.innerText = text;
	chatLine.appendChild(chatContent);
	return chatLine;
};

const handleChat = async () => {
	userQuestion = chatInput.value.trim();
	chatInput.value = "";
	if (!userQuestion) return;
	sendQuestionBtn.style.opacity = 0.5;
	sendQuestionBtn.removeEventListener("click", handleChat);
	chatInput.removeEventListener("keyup", enterHandler);
	chatBox.appendChild(createUserChatLine(userQuestion));
	chatBox.appendChild(createAnswerChatLine("Loading..."));
	if (chatBox.scrollHeight > 0) {
		chatBox.scrollTop = chatBox.scrollHeight;
	}

	try {
		const response = await fetch("https://localhost:7063/chat/send", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify({
				message: userQuestion,
			}),
		});

		const reader = response.body.getReader();

		chatBox.lastChild.lastChild.innerText = "";
		while (true) {
			const { value, done } = await reader.read();

			if (done) break;

			chatBox.lastChild.lastChild.innerText += new TextDecoder().decode(
				value
			);

			if (chatBox.scrollHeight > 0) {
				chatBox.scrollTop = chatBox.scrollHeight;
			}
		}
	} catch (error) {
		console.log(error);
	}

	sendQuestionBtn.addEventListener("click", handleChat);
	chatInput.addEventListener("keyup", enterHandler);
	sendQuestionBtn.style.opacity = 1;
};

const enterHandler = (event) => {
	if (event.key === "Enter" && !event.shiftKey) {
		event.preventDefault();
		handleChat();
	}
};

sendQuestionBtn.addEventListener("click", handleChat);
chatInput.addEventListener("keyup", enterHandler);
