// ARQUIVO: dashboard.js (VERSÃO ATUALIZADA COM FILTROS E PAGINAÇÃO)

const DashboardApp = {
    // --- 1. Propriedades (Estado) ---
    API_BASE_URL: 'http://localhost:5147/api',
    token: null,
    userInfo: {
        id: null,
        role: null,
        name: null
    },
    paginaAtual: 1, // NOVO: Controla a página atual
    tamanhoPagina: 10, // NOVO: Quantos itens por página

    // --- 2. Elementos DOM (Views) ---
    elements: {
        sidebarNav: document.getElementById('sidebar-nav'),
        userAvatar: document.getElementById('user-avatar'),
        userName: document.getElementById('user-name'),
        userRole: document.getElementById('user-role'),
        mainTitle: document.getElementById('main-title'),
        tableTitle: document.getElementById('table-title'),
        tableHeader: document.getElementById('table-header-row'),
        tableBody: document.getElementById('tabela-chamados-corpo'),
        menuToggle: document.querySelector('.menu-toggle'),
        sidebar: document.querySelector('.sidebar'),
        // Chat
        openChatBtn: document.getElementById('open-chat'),
        closeChatBtn: document.getElementById('close-chat'),
        chatOverlay: document.getElementById('chat-overlay'),
        chatSendBtn: document.getElementById('chat-send-btn'),
        chatInput: document.getElementById('chat-input-text'),
        chatMessages: document.querySelector('.chat-messages'),
        // Filtros
        formFiltros: document.getElementById('form-filtros'),
        filtroStatus: document.getElementById('filtro-status'),
        filtroPrioridade: document.getElementById('filtro-prioridade'),
        filtroTipo: document.getElementById('filtro-tipo'),
        filtroDataInicio: document.getElementById('filtro-data-inicio'),
        filtroDataFim: document.getElementById('filtro-data-fim'),
        btnFiltrar: document.getElementById('btn-filtrar'),
        btnLimparFiltros: document.getElementById('btn-limpar-filtros'),
        // Paginação (NOVOS)
        paginationControls: document.getElementById('pagination-controls'),
        btnPaginaAnterior: document.getElementById('btn-pagina-anterior'),
        btnPaginaProxima: document.getElementById('btn-pagina-proxima'),
        infoPaginacao: document.getElementById('info-paginacao')
    },

    // --- 3. Métodos (Controladores) ---

    init: function() {
        this.token = localStorage.getItem('jwt_token');
        if (!this.token) {
            alert("Acesso não autorizado. Por favor, faça o login.");
            window.location.href = 'login.html';
            return;
        }

        this.userInfo = this.decodificarToken(this.token);
        if (!this.userInfo) return;

        this.renderLayout();
        this.popularFiltros(); 
        this.loadChamados();
        this.attachEventListeners();
        feather.replace();
    },

    decodificarToken: function(token) {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const roleClaimKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            const userRole = payload[roleClaimKey] || payload.role; 
            if (!userRole) throw new Error("Token inválido: Role (perfil) não encontrado.");

            return {
                id: parseInt(payload.nameid), 
                role: userRole,
                name: payload.sub
            };
        } catch (e) {
            console.error("Erro ao decodificar token:", e);
            localStorage.removeItem('jwt_token');
            window.location.href = 'login.html';
            return null;
        }
    },

    renderLayout: function() {
        const { role, name } = this.userInfo;
        const { sidebarNav, userAvatar, userName, userRole, mainTitle, tableTitle } = this.elements;

        userName.textContent = name;
        userRole.textContent = role;
        userAvatar.textContent = name.substring(0, 2).toUpperCase();

        let pageTitle = (role === 'Colaborador') ? "Meus Chamados" : "Dashboard";
        let chamadosTitle = (role === 'Colaborador') ? "Meus Chamados Recentes" : "Todos os Chamados";
        mainTitle.textContent = pageTitle;
        tableTitle.textContent = chamadosTitle;

        const sidebarLinks = [
            { text: 'Dashboard', href: 'dashboard.html', icon: 'home', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Gerenciar Usuários', href: 'gerenciar_usuarios.html', icon: 'users', roles: ['Administrador'] },
            { text: 'Relatórios', href: 'relatorio.html', icon: 'bar-chart-2', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Configurações', href: 'configuracoes.html', icon: 'settings', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'FAQ', href: 'FAQ.html', icon: 'help-circle', roles: ['Administrador', 'Tecnico', 'Colaborador'] }
        ];
        
        const ul = document.createElement('ul');
        const currentPage = window.location.pathname.split('/').pop();

        sidebarLinks.filter(link => link.roles.includes(role)).forEach(link => {
            const li = document.createElement('li');
            const isActive = (link.href === currentPage) ? 'class="active"' : '';
            li.innerHTML = `<a href="${link.href}" ${isActive}><i data-feather="${link.icon}"></i> ${link.text}</a>`;
            ul.appendChild(li);
        });
        sidebarNav.innerHTML = '';
        sidebarNav.appendChild(ul);
    },

    popularFiltros: async function() {
        this._fetchDropdown('status', this.elements.filtroStatus, 'Todos');
        this._fetchDropdown('prioridades', this.elements.filtroPrioridade, 'Todas');
        this._fetchDropdown('tipos', this.elements.filtroTipo, 'Todos');
    },

    _fetchDropdown: async function(endpoint, selectEl, defaultText) {
        try {
            const response = await fetch(`${this.API_BASE_URL}/${endpoint}`, {
                headers: { 'Authorization': `Bearer ${this.token}` }
            });
            if (!response.ok) throw new Error(`Erro ao buscar ${endpoint}`);
            const data = await response.json();
            
            while (selectEl.options.length > 1) selectEl.remove(1);
            
            data.forEach(item => {
                const option = document.createElement('option');
                option.value = item.id;
                option.textContent = item.nome;
                selectEl.appendChild(option);
            });
        } catch (error) {
            console.error(error);
            selectEl.innerHTML = `<option value="">Erro ao carregar</option>`;
        }
    },


    /**
     * (MODIFICADO) Carrega os chamados da API, agora com filtros E paginação.
     */
    loadChamados: async function() {
        const { tableBody, tableHeader } = this.elements;
        
        const showSolicitante = (this.userInfo.role === 'Administrador' || this.userInfo.role === 'Tecnico');
        let headerHtml = `
            <th>ID</th> <th>Assunto</th>
            ${showSolicitante ? '<th>Solicitante</th>' : ''}
            <th>Status</th> <th>Prioridade</th> <th>Data</th> <th>Ações</th>
        `;
        tableHeader.innerHTML = headerHtml;
        const colspan = showSolicitante ? 7 : 6;

        tableBody.innerHTML = `<tr><td colspan="${colspan}" style="text-align:center;">Carregando chamados...</td></tr>`;
        
        // --- Montagem dos Parâmetros (Filtros + Paginação) ---
        const params = new URLSearchParams();
        const { filtroStatus, filtroPrioridade, filtroTipo, filtroDataInicio, filtroDataFim } = this.elements;

        if (filtroStatus.value) params.append('statusId', filtroStatus.value);
        if (filtroPrioridade.value) params.append('prioridadeId', filtroPrioridade.value);
        if (filtroTipo.value) params.append('tipoId', filtroTipo.value);
        if (filtroDataInicio.value) params.append('dataInicio', filtroDataInicio.value);
        if (filtroDataFim.value) params.append('dataFim', filtroDataFim.value);

        // Adiciona parâmetros de paginação
        params.append('pagina', this.paginaAtual);
        params.append('tamanhoPagina', this.tamanhoPagina);

        const queryString = params.toString();
        const url = `${this.API_BASE_URL}/chamados?${queryString}`;
        
        try {
            const response = await fetch(url, {
                headers: { 'Authorization': `Bearer ${this.token}` }
            });
            if (response.status === 401) { window.location.href = 'login.html'; return; }
            if (!response.ok) throw new Error('Falha ao buscar dados.');

            // A RESPOSTA AGORA É UM OBJETO { dados: [], totalItens: ... }
            const responseData = await response.json();
            
            this.renderTableRows(responseData.dados, colspan); // Passa apenas os DADOS
            this.renderPaginacao(responseData); // Passa a METADATA

        } catch (error) {
            tableBody.innerHTML = `<tr><td colspan="${colspan}" style="text-align:center;">Erro ao carregar chamados.</td></tr>`;
            console.error(error);
        } finally {
            feather.replace(); // Renderiza os ícones (olho, lixeira, etc)
        }
    },

    /**
     * Renderiza as linhas da tabela. (Recebe a lista de 'dados')
     */
    renderTableRows: function(chamados, colspan) {
        const { tableBody } = this.elements;
        tableBody.innerHTML = ''; 

        if (chamados.length === 0) {
            tableBody.innerHTML = `<tr><td colspan="${colspan}" style="text-align:center;">Nenhum chamado encontrado.</td></tr>`;
            return;
        }

        const showSolicitante = (this.userInfo.role === 'Administrador' || this.userInfo.role === 'Tecnico');

        chamados.forEach(chamado => {
            const tr = document.createElement('tr');
            
            const solicitanteHtml = showSolicitante ? `<td>${chamado.usuario.nome}</td>` : '';
            const actionButtonsHtml = this.getActionButtonHTML(chamado);

            tr.innerHTML = `
                <td>#${chamado.id}</td>
                <td>${chamado.titulo}</td>
                ${solicitanteHtml}
                <td><span class="status status-${chamado.statusChamado.nome.toLowerCase().replace(' ', '-')}">${chamado.statusChamado.nome}</span></td>
                <td>${chamado.prioridade.nome}</td>
                <td>${new Date(chamado.dataAbertura).toLocaleDateString()}</td>
                <td class="action-buttons">
                    ${actionButtonsHtml}
                </td>
            `;
            tableBody.appendChild(tr);
        });
    },
    
    /**
     * (NOVO) Renderiza os controles de paginação.
     */
    renderPaginacao: function(data) {
        const { totalItens, paginaAtual, totalPaginas } = data;
        const { paginationControls, btnPaginaAnterior, btnPaginaProxima, infoPaginacao } = this.elements;

        // Se não houver páginas (ou só 1), esconde os controles
        if (totalPaginas <= 1) {
            paginationControls.style.display = 'none';
            return;
        }
        
        // Mostra os controles
        paginationControls.style.display = 'flex';
        
        // Atualiza o texto
        infoPaginacao.textContent = `Página ${paginaAtual} de ${totalPaginas} (${totalItens} itens)`;

        // Habilita/Desabilita botões
        btnPaginaAnterior.disabled = (paginaAtual === 1);
        btnPaginaProxima.disabled = (paginaAtual === totalPaginas);
    },


    getActionButtonHTML: function(chamado) {
        const { role, id } = this.userInfo;
        const isOwner = (chamado.idUsuario === id);

        let html = `<a href="chamado.html?id=${chamado.id}" title="Ver detalhes"><i data-feather="eye"></i></a>`;

        if (role === 'Administrador' || (role === 'Colaborador' && isOwner)) {
            html += ` <button class="btn-delete" data-id="${chamado.id}" title="Excluir"><i data-feather="trash-2"></i></button>`;
        }
        
        return html;
    },

    attachEventListeners: function() {
        this.elements.menuToggle?.addEventListener('click', () => {
            this.elements.sidebar?.classList.toggle('open');
        });

        // Chat
        this.elements.openChatBtn?.addEventListener('click', () => this.elements.chatOverlay.style.display = 'flex');
        this.elements.closeChatBtn?.addEventListener('click', () => this.elements.chatOverlay.style.display = 'none');
        this.elements.chatSendBtn?.addEventListener('click', () => this.enviarPerguntaParaIa());
        this.elements.chatInput?.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') this.enviarPerguntaParaIa();
        });
        
        // Deleção
        this.elements.tableBody.addEventListener('click', (event) => {
            const deleteButton = event.target.closest('.btn-delete');
            if (deleteButton) this.handleDelete(deleteButton.dataset.id);
        });

        // --- LISTENERS MODIFICADOS E NOVOS ---
        
        // Botão Filtrar: Reseta para a página 1 e carrega
        this.elements.btnFiltrar.addEventListener('click', () => {
            this.paginaAtual = 1; // Reseta para a primeira página
            this.loadChamados();
        });

        // Botão Limpar: Limpa campos, reseta para pág 1 e carrega
        this.elements.btnLimparFiltros.addEventListener('click', () => {
            this.elements.filtroStatus.value = "";
            this.elements.filtroPrioridade.value = "";
            this.elements.filtroTipo.value = "";
            this.elements.filtroDataInicio.value = "";
            this.elements.filtroDataFim.value = "";
            this.paginaAtual = 1; // Reseta para a primeira página
            this.loadChamados();
        });
        
        // Botões de Paginação
        this.elements.btnPaginaAnterior.addEventListener('click', () => {
            if (this.paginaAtual > 1) {
                this.paginaAtual--;
                this.loadChamados();
            }
        });

        this.elements.btnPaginaProxima.addEventListener('click', () => {
            // A lógica de desabilitar o botão no 'renderPaginacao' já previne isso,
            // mas é uma boa prática verificar aqui também.
            this.paginaAtual++;
            this.loadChamados();
        });
    },

    handleDelete: async function(chamadoId) {
        if (!confirm(`Tem certeza que deseja excluir o chamado #${chamadoId}? Esta ação não pode ser desfeita.`)) {
            return;
        }
        try {
            const response = await fetch(`${this.API_BASE_URL}/chamados/${chamadoId}`, {
                method: 'DELETE',
                headers: { 'Authorization': `Bearer ${this.token}` }
            });
            if (response.ok) {
                alert('Chamado excluído com sucesso.');
                // Se excluir o último item de uma página, ela ficará vazia.
                // O ideal é recarregar a página atual ou voltar para a 1.
                // Vamos recarregar a página atual por simplicidade.
                this.loadChamados(); 
            } else {
                const errorData = await response.text();
                if(response.status === 403) alert(errorData);
                else throw new Error(errorData || 'Falha ao excluir o chamado.');
            }
        } catch (error) {
            console.error('Erro ao excluir:', error);
            alert(error.message);
        }
    },

    enviarPerguntaParaIa: async function() {
        // ... (nenhuma mudança nesta função) ...
        const pergunta = this.elements.chatInput.value.trim();
        if (pergunta === '') return;
        this.addChatMessage(pergunta, 'user');
        this.elements.chatInput.value = '';
        this.elements.chatInput.disabled = true;
        this.elements.chatSendBtn.disabled = true;
        try {
            const response = await fetch(`${this.API_BASE_URL}/ia/consulta`, {
                method: 'POST',
                headers: { 'Authorization': `Bearer ${this.token}`, 'Content-Type': 'application/json' },
                body: JSON.stringify({ pergunta })
            });
            if (!response.ok) throw new Error('Falha na consulta à IA.');
            const data = await response.json();
            this.addChatMessage(data.solucao, 'bot');
        } catch (error) {
            this.addChatMessage("Desculpe, não consigo me conectar ao assistente agora.", 'bot');
        } finally {
            this.elements.chatInput.disabled = false;
            this.elements.chatSendBtn.disabled = false;
            this.elements.chatInput.focus();
        }
    },

    addChatMessage: function(texto, tipo) {
        // ... (nenhuma mudança nesta função) ...
        if (!this.elements.chatMessages) return;
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message', tipo);
        messageDiv.textContent = texto;
        this.elements.chatMessages.appendChild(messageDiv);
        this.elements.chatMessages.scrollTop = this.elements.chatMessages.scrollHeight;
    }
};

// --- Ponto de Entrada ---
document.addEventListener('DOMContentLoaded', () => {
    DashboardApp.init();
});