document.addEventListener('DOMContentLoaded', () => {
    
    // --- Configurações e Guarda de Rota ---
    const API_BASE_URL = 'http://localhost:5147/api'; // Verifique se a porta está correta!
    const token = localStorage.getItem('jwt_token');

    if (!token) {
        alert("Acesso não autorizado. Por favor, faça o login.");
        window.location.href = 'login.html';
        return;
    }

    // --- Elementos da Página ---
    const corpoTabela = document.getElementById('tabela-chamados-corpo');
    const nomeUsuarioEl = document.getElementById('nome-usuario');
    const perfilUsuarioEl = document.getElementById('perfil-usuario');
    
    // Elementos do Chat
    const openChatBtn = document.getElementById('open-chat'); // Botão flutuante
    const sidebarConsultarIaBtn = document.getElementById('sidebar-consultar-ia'); // <-- NOVO: Botão no menu
    const closeChatBtn = document.getElementById('close-chat');
    const chatOverlay = document.getElementById('chat-overlay');
    const chatMessagesContainer = document.querySelector('.chat-messages');
    const chatInput = document.getElementById('chat-input-text');
    const chatSendBtn = document.getElementById('chat-send-btn');
    
    // Elementos do Sidebar
    const menuToggle = document.querySelector('.menu-toggle');
    const sidebar = document.querySelector('.sidebar');

    // --- Funções Auxiliares ---
    function decodificarToken(token) {
        try {
            return JSON.parse(atob(token.split('.')[1]));
        } catch (e) {
            console.error("Erro ao decodificar token:", e);
            window.location.href = 'login.html';
            return null;
        }
    }

    function exibirMensagem(texto, tipo) {
        if (!chatMessagesContainer) return;
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message', tipo);
        messageDiv.textContent = texto;
        chatMessagesContainer.appendChild(messageDiv);
        chatMessagesContainer.scrollTop = chatMessagesContainer.scrollHeight;
    }

    // --- Funções Principais ---
    async function carregarChamados() {
        // ... (esta função continua exatamente a mesma)
        if (!corpoTabela) return;
        corpoTabela.innerHTML = '<tr><td colspan="7" style="text-align:center;">Carregando chamados...</td></tr>';
        try {
            const response = await fetch(`${API_BASE_URL}/chamados`, {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            if (response.status === 401) { window.location.href = 'login.html'; return; }
            if (!response.ok) throw new Error('Falha ao buscar dados.');
            const chamados = await response.json();
            corpoTabela.innerHTML = ''; 
            if (chamados.length === 0) {
                corpoTabela.innerHTML = '<tr><td colspan="7" style="text-align:center;">Nenhum chamado encontrado.</td></tr>';
                return;
            }
            chamados.forEach(chamado => {
                const tr = document.createElement('tr');
                const colunaSolicitante = document.querySelector('.col-solicitante') ? `<td>${chamado.usuario.nome}</td>` : '';
                tr.innerHTML = `<td>#${chamado.id}</td><td>${chamado.titulo}</td>${colunaSolicitante}<td><span class="status status-aberto">${chamado.statusChamado.nome}</span></td><td>${chamado.prioridade.nome}</td><td>${new Date(chamado.dataAbertura).toLocaleDateString()}</td><td class="action-buttons"><a href="chamado.html?id=${chamado.id}" title="Ver detalhes"><i data-feather="eye"></i></a><a href="#" title="Editar"><i data-feather="edit-2"></i></a></td>`;
                corpoTabela.appendChild(tr);
            });
        } catch (error) {
            corpoTabela.innerHTML = '<tr><td colspan="7" style="text-align:center;">Erro ao carregar chamados.</td></tr>';
        } finally {
            feather.replace();
        }
    }

    async function enviarPerguntaParaIa() {
        // ... (esta função continua exatamente a mesma)
        if (!chatInput) return;
        const pergunta = chatInput.value.trim();
        if (pergunta === '') return;
        exibirMensagem(pergunta, 'user');
        chatInput.value = '';
        chatInput.disabled = true;
        chatSendBtn.disabled = true;
        try {
            const response = await fetch(`${API_BASE_URL}/ia/consulta`, {
                method: 'POST',
                headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
                body: JSON.stringify({ pergunta })
            });
            if (!response.ok) throw new Error('Falha na consulta à IA.');
            const data = await response.json();
            exibirMensagem(data.solucao, 'bot');
        } catch (error) {
            exibirMensagem("Desculpe, não consigo me conectar ao assistente agora.", 'bot');
        } finally {
            chatInput.disabled = false;
            chatSendBtn.disabled = false;
            chatInput.focus();
        }
    }

    // --- INICIALIZAÇÃO DA PÁGINA ---
    
    // 1. Personaliza a interface
    const payload = decodificarToken(token);
    if (payload) {
        if (nomeUsuarioEl) nomeUsuarioEl.textContent = payload.sub;
        if (perfilUsuarioEl) perfilUsuarioEl.textContent = payload.role;
    }

    // 2. Carrega os dados da tabela
    carregarChamados();
    
    // 3. Configura os gatilhos de eventos (clicks, etc.)
    
    // CORREÇÃO: Adicionamos a lógica para os dois botões abrirem o chat
    if (openChatBtn) openChatBtn.addEventListener('click', () => chatOverlay.style.display = 'flex');
    if (sidebarConsultarIaBtn) {
        sidebarConsultarIaBtn.addEventListener('click', (e) => {
            e.preventDefault(); // Previne que o link '#' recarregue a página
            chatOverlay.style.display = 'flex';
        });
    }
    
    if (closeChatBtn) closeChatBtn.addEventListener('click', () => chatOverlay.style.display = 'none');
    if (chatSendBtn) chatSendBtn.addEventListener('click', enviarPerguntaParaIa);
    if (chatInput) chatInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') enviarPerguntaParaIa();
    });
    if (menuToggle && sidebar) {
        menuToggle.addEventListener('click', () => sidebar.classList.toggle('open'));
    }

    // 4. Renderiza todos os ícones da página
    feather.replace();
});