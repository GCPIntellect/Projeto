// ARQUIVO: abrir_chamado.js (VERSÃO ATUALIZADA - Link do FAQ adicionado)

const AbrirChamadoApp = {
    // --- 1. Propriedades (Estado) ---
    API_BASE_URL: 'http://localhost:5147/api',
    token: null,
    userInfo: {
        id: null,
        role: null,
        name: null
    },

    // --- 2. Elementos DOM (Views) ---
    elements: {
        // Layout
        sidebarNav: document.getElementById('sidebar-nav'),
        userAvatar: document.getElementById('user-avatar'),
        userName: document.getElementById('user-name'),
        userRole: document.getElementById('user-role'),
        // Form
        form: document.getElementById('form-abrir-chamado'),
        btnSubmit: document.getElementById('btn-submit'),
        messageContainer: document.getElementById('message-container'),
        // Campos Admin
        groupPrioridade: document.getElementById('group-prioridade'),
        groupStatus: document.getElementById('group-status'),
        // Anexos
        inputAnexos: document.getElementById('anexos'),
        labelAnexos: document.getElementById('anexos-label'),
        previewAnexos: document.getElementById('anexos-preview')
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
        this.popularDropdowns();
        this.applyPermissions();
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
        const { sidebarNav, userAvatar, userName, userRole } = this.elements;
        userName.textContent = name;
        userRole.textContent = role;
        userAvatar.textContent = name.substring(0, 2).toUpperCase();

        // ==========================================================
        // --- INÍCIO DA CORREÇÃO (Link do FAQ) ---
        // ==========================================================
        const sidebarLinks = [
            { text: 'Dashboard', href: 'dashboard.html', icon: 'home', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Gerenciar Usuários', href: 'gerenciar_usuarios.html', icon: 'users', roles: ['Administrador'] },
            { text: 'Relatórios', href: 'relatorio.html', icon: 'bar-chart-2', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'Configurações', href: 'configuracoes.html', icon: 'settings', roles: ['Administrador', 'Tecnico', 'Colaborador'] },
            { text: 'FAQ', href: 'FAQ.html', icon: 'help-circle', roles: ['Administrador', 'Tecnico', 'Colaborador'] } // <-- MOVIDO PARA O FINAL
        ];
        // ==========================================================
        // --- FIM DA CORREÇÃO ---
        // ==========================================================
        
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

    applyPermissions: function() {
        const { role } = this.userInfo;
        if (role === 'Administrador' || role === 'Tecnico') {
            this.elements.groupPrioridade.style.display = 'block';
            this.elements.groupStatus.style.display = 'block';
        }
    },

    popularDropdowns: async function() {
        const fetchDropdown = async (endpoint, selectId, valorPadrao = null) => {
            const selectEl = document.getElementById(selectId);
            try {
                const response = await fetch(`${this.API_BASE_URL}/${endpoint}`, {
                    headers: { 'Authorization': `Bearer ${this.token}` }
                });
                if (!response.ok) throw new Error(`Erro ao buscar ${endpoint}`);
                const data = await response.json();
                
                selectEl.innerHTML = ''; 
                
                const defaultOption = document.createElement('option');
                defaultOption.value = "";
                defaultOption.textContent = valorPadrao || "Selecione...";
                if (!valorPadrao) defaultOption.disabled = true;
                selectEl.appendChild(defaultOption);

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
        };

        fetchDropdown('tipos', 'tipo');
        fetchDropdown('categorias', 'categoria');
        fetchDropdown('prioridades', 'prioridade', 'Padrão (Baixo)');
        fetchDropdown('status', 'status', 'Padrão (Em Andamento)');
    },
    
    attachEventListeners: function() {
        this.elements.form.addEventListener('submit', (event) => {
            event.preventDefault();
            this.enviarNovoChamado();
        });

        this.elements.inputAnexos.addEventListener('change', () => {
            this.atualizarPreviewAnexos();
        });
    },

    atualizarPreviewAnexos: function() {
        const { inputAnexos, previewAnexos, labelAnexos } = this.elements;
        previewAnexos.innerHTML = ''; 
        
        if (inputAnexos.files.length === 0) {
            labelAnexos.querySelector('span').textContent = 'Clique para carregar arquivos (imagens, logs, etc.)';
            return;
        }

        labelAnexos.querySelector('span').textContent = `${inputAnexos.files.length} arquivo(s) selecionado(s):`;

        Array.from(inputAnexos.files).forEach(file => {
            const fileItem = document.createElement('div');
            fileItem.className = 'anexo-preview-item';
            fileItem.textContent = file.name;
            previewAnexos.appendChild(fileItem);
        });
    },

    enviarNovoChamado: async function() {
        const { form, messageContainer, btnSubmit } = this.elements;
        messageContainer.innerHTML = '';
        btnSubmit.disabled = true;
        btnSubmit.textContent = 'Enviando...';

        const formData = new FormData(form);

        formData.append('Titulo', formData.get('assunto'));
        formData.append('Descricao', formData.get('descricao'));
        formData.append('IdTipo', formData.get('tipo'));
        formData.append('IdCategoria', formData.get('categoria_id'));
        formData.append('IdPrioridade', formData.get('prioridade_id'));
        formData.append('IdStatus', formData.get('status_id'));
        
        try {
            const response = await fetch(`${this.API_BASE_URL}/chamados`, {
                method: 'POST',
                headers: { 
                    'Authorization': `Bearer ${this.token}`
                },
                body: formData 
            });

            if (response.ok) {
                messageContainer.innerHTML = '<p class="success-text">Chamado criado com sucesso!</p>';
                
                setTimeout(() => window.location.href = 'dashboard.html', 2000);

            } else {
                const erro = await response.text(); 
                throw new Error(erro || 'Falha ao criar o chamado.');
            }
        } catch (error) {
            messageContainer.innerHTML = `<p class="error-text">${error.message}</p>`;
            btnSubmit.disabled = false;
            btnSubmit.textContent = 'Criar Chamado';
        }
    }
};

// --- Ponto de Entrada ---
document.addEventListener('DOMContentLoaded', () => {
    AbrirChamadoApp.init();
});