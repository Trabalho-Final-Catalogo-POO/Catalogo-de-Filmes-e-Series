using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CatalogoSimples
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Senha { get; set; }
        public List<Midia> Favoritos { get; } = new List<Midia>();

        public Usuario(string nome, string senha)
        {
            Nome = nome;
            Senha = senha;
        }

        public void AdicionarFavorito(Midia midia)
        {
            if (!Favoritos.Contains(midia))
            {
                Favoritos.Add(midia);
            }
        }

        public void RemoverFavorito(Midia midia)
        {
            Favoritos.Remove(midia);
        }

        public bool IsFavorito(Midia midia)
        {
            return Favoritos.Contains(midia);
        }
    }

    public abstract class Midia
    {
        public string Nome { get; set; }
        public string Genero { get; set; }
        public int Ano { get; set; }
        public double Duracao { get; set; }

        protected Midia(string nome, string genero, int ano, double duracao)
        {
            Nome = nome;
            Genero = genero;
            Ano = ano;
            Duracao = duracao;
        }

        public abstract string GetDescricao();
    }

    public class Filme : Midia
    {
        public string Diretor { get; set; }

        public Filme(string nome, string genero, int ano, double duracao, string diretor)
            : base(nome, genero, ano, duracao)
        {
            Diretor = diretor;
        }

        public override string GetDescricao()
        {
            return $"Filme: {Nome} ({Ano})\nGênero: {Genero}\nDuração: {Duracao} min\nDiretor: {Diretor}";
        }
    }

    public class Serie : Midia
    {
        public int Temporadas { get; set; }
        public int Episodios { get; set; }

        public Serie(string nome, string genero, int ano, double duracao, int temporadas, int episodios)
            : base(nome, genero, ano, duracao)
        {
            Temporadas = temporadas;
            Episodios = episodios;
        }

        public override string GetDescricao()
        {
            return $"Série: {Nome} ({Ano})\nGênero: {Genero}\nDuração ep.: {Duracao} min\nTemporadas: {Temporadas}\nEpisódios: {Episodios}";
        }
    }

    public static class Sistema
    {
        public static List<Usuario> Usuarios = new List<Usuario>();
        public static List<Midia> Midias = new List<Midia>();
        public static Usuario UsuarioLogado = null;

        static Sistema()
        {
            // Inicializar com alguns dados para teste
            Usuarios.Add(new Usuario("admin", "admin"));
            Usuarios.Add(new Usuario("user", "123"));
            
            Midias.Add(new Filme("Matrix", "Ficção Científica", 1999, 136, "Lana Wachowski"));
            Midias.Add(new Serie("Stranger Things", "Suspense", 2016, 50, 4, 34));
        }
    }

    public class CadastroUsuarioForm : Form
    {
        private TextBox txtNome;
        private TextBox txtSenha;

        public CadastroUsuarioForm()
        {
            this.Text = "Cadastro de Usuário";
            this.Size = new Size(300, 180);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            Label lblNome = new Label { Text = "Nome:", Top = 20, Left = 20, Width = 80, ForeColor = Color.White };
            txtNome = new TextBox { Top = 20, Left = 100, Width = 150, BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White };

            Label lblSenha = new Label { Text = "Senha:", Top = 60, Left = 20, Width = 80, ForeColor = Color.White };
            txtSenha = new TextBox { Top = 60, Left = 100, Width = 150, PasswordChar = '*', BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White };

            Button btnCadastrar = new Button { 
                Text = "Cadastrar", 
                Top = 100, 
                Left = 100, 
                Width = 100, 
                BackColor = Color.DarkRed, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat 
            };
            btnCadastrar.FlatAppearance.BorderSize = 0;
            btnCadastrar.Click += BtnCadastrar_Click;

            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);
            this.Controls.Add(lblSenha);
            this.Controls.Add(txtSenha);
            this.Controls.Add(btnCadastrar);
        }

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string senha = txtSenha.Text;

            if (nome == "" || senha == "")
            {
                MessageBox.Show("Preencha todos os campos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Sistema.Usuarios.Exists(u => u.Nome == nome))
            {
                MessageBox.Show("Usuário já existe!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Sistema.Usuarios.Add(new Usuario(nome, senha));
            MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }

    public class CadastroMidiaForm : Form
    {
        private ComboBox cmbTipo;
        private TextBox txtNome, txtGenero, txtAno, txtDuracao, txtDiretor, txtTemporadas, txtEpisodios;

        public CadastroMidiaForm()
        {
            this.Text = "Cadastro de Mídia";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            // Criação dos controles
            Label lblTipo = new Label { Text = "Tipo:", Top = 20, Left = 20, Width = 100, ForeColor = Color.White };
            cmbTipo = new ComboBox { Top = 20, Left = 130, Width = 200, BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White };
            cmbTipo.Items.AddRange(new string[] { "Filme", "Série" });

            // Criar todos os campos antes de configurar o evento
            txtNome = CriarCampo("Nome", 60);
            txtGenero = CriarCampo("Gênero", 100);
            txtAno = CriarCampo("Ano", 140);
            txtDuracao = CriarCampo("Duração (min)", 180);
            txtDiretor = CriarCampo("Diretor", 220);
            txtTemporadas = CriarCampo("Temporadas", 260);
            txtEpisodios = CriarCampo("Episódios", 300);

            Button btnCadastrar = new Button { 
                Text = "Cadastrar", 
                Top = 340, 
                Left = 150, 
                Width = 100, 
                BackColor = Color.DarkRed, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat 
            };
            btnCadastrar.FlatAppearance.BorderSize = 0;
            btnCadastrar.Click += BtnCadastrar_Click;

            this.Controls.AddRange(new Control[] {
                lblTipo, cmbTipo, btnCadastrar
            });

            // Configurar evento DEPOIS de criar todos os controles
            cmbTipo.SelectedIndexChanged += (s, e) => AtualizarCampos();
            cmbTipo.SelectedIndex = 0; // Isso disparará o evento
        }

        private TextBox CriarCampo(string labelText, int top)
        {
            Label lbl = new Label { Text = labelText + ":", Top = top, Left = 20, Width = 100, ForeColor = Color.White };
            TextBox txt = new TextBox { Top = top, Left = 130, Width = 200, BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White };
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
            return txt;
        }

        private void AtualizarCampos()
        {
            bool isFilme = cmbTipo.SelectedItem?.ToString() == "Filme";
            
            if (txtDiretor != null) txtDiretor.Enabled = isFilme;
            if (txtTemporadas != null) txtTemporadas.Enabled = !isFilme;
            if (txtEpisodios != null) txtEpisodios.Enabled = !isFilme;

            if (isFilme)
            {
                if (txtTemporadas != null) txtTemporadas.Text = "";
                if (txtEpisodios != null) txtEpisodios.Text = "";
            }
            else
            {
                if (txtDiretor != null) txtDiretor.Text = "";
            }
        }

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            string tipo = cmbTipo.SelectedItem?.ToString();
            string nome = txtNome.Text.Trim();
            string genero = txtGenero.Text.Trim();

            // Validações
            if (string.IsNullOrWhiteSpace(nome)) {
                MessageBox.Show("Nome é obrigatório!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (string.IsNullOrWhiteSpace(genero)) {
                MessageBox.Show("Gênero é obrigatório!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (!int.TryParse(txtAno.Text, out int ano) || ano < 1888 || ano > DateTime.Now.Year + 5) {
                MessageBox.Show("Ano inválido! Use um ano entre 1888 e " + (DateTime.Now.Year + 5), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (!double.TryParse(txtDuracao.Text, out double duracao) || duracao <= 0) {
                MessageBox.Show("Duração inválida! Use números maiores que zero.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Processar cadastro
            if (tipo == "Filme")
            {
                string diretor = txtDiretor.Text.Trim();
                if (string.IsNullOrWhiteSpace(diretor)) {
                    MessageBox.Show("Diretor é obrigatório para filmes!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                Sistema.Midias.Add(new Filme(nome, genero, ano, duracao, diretor));
                MessageBox.Show($"Filme '{nome}' cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!int.TryParse(txtTemporadas.Text, out int temporadas) || temporadas <= 0) {
                    MessageBox.Show("Temporadas inválidas! Use números maiores que zero.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (!int.TryParse(txtEpisodios.Text, out int episodios) || episodios <= 0) {
                    MessageBox.Show("Episódios inválidos! Use números maiores que zero.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                Sistema.Midias.Add(new Serie(nome, genero, ano, duracao, temporadas, episodios));
                MessageBox.Show($"Série '{nome}' cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }
    }

    public class LoginForm : Form
    {
        private TextBox txtUsuario;
        private TextBox txtSenha;
        
        public LoginForm()
        {
            this.Text = "Login";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            
            Label lblUsuario = new Label { Text = "Nome:", Top = 20, Left = 20, Width = 80, ForeColor = Color.White };
            txtUsuario = new TextBox { Top = 20, Left = 100, Width = 150, BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White };

            Label lblSenha = new Label { Text = "Senha:", Top = 60, Left = 20, Width = 80, ForeColor = Color.White };
            txtSenha = new TextBox { Top = 60, Left = 100, Width = 150, PasswordChar = '*', BackColor = Color.FromArgb(50, 50, 50), ForeColor = Color.White };
            Button btnLogin = new Button { 
                Text = "Login", 
                Top = 100, 
                Left = 100, 
                Width = 80, 
                BackColor = Color.DarkRed, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat 
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            
            this.Controls.Add(lblUsuario);
            this.Controls.Add(txtUsuario);
            this.Controls.Add(lblSenha);
            this.Controls.Add(txtSenha);
            this.Controls.Add(btnLogin);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var user = Sistema.Usuarios.Find(u => u.Nome == usuario && u.Senha == senha);
            
            if (user != null)
            {
                Sistema.UsuarioLogado = user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuário ou senha inválidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class DetalhesForm : Form
    {
        private Midia midia;
        
        public DetalhesForm(Midia midia)
        {
            this.midia = midia;
            this.Text = "Detalhes";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            
            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = midia.Nome;
            lblTitulo.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.Red;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 40;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            
            // Detalhes
            TextBox txtDetalhes = new TextBox();
            txtDetalhes.Text = midia.GetDescricao();
            txtDetalhes.Multiline = true;
            txtDetalhes.Dock = DockStyle.Fill;
            txtDetalhes.BackColor = Color.FromArgb(50, 50, 50);
            txtDetalhes.ForeColor = Color.White;
            txtDetalhes.BorderStyle = BorderStyle.None;
            txtDetalhes.ReadOnly = true;
            txtDetalhes.ScrollBars = ScrollBars.Vertical;
            txtDetalhes.Padding = new Padding(10);
            
            // Botão de favorito
            Button btnFavorito = new Button();
            btnFavorito.Name = "btnFavorito";
            btnFavorito.Dock = DockStyle.Bottom;
            btnFavorito.Height = 40;
            btnFavorito.FlatStyle = FlatStyle.Flat;
            btnFavorito.FlatAppearance.BorderSize = 0;
            AtualizarBotaoFavorito(btnFavorito);
            
            btnFavorito.Click += (s, e) => 
            {
                if (Sistema.UsuarioLogado == null) return;
                
                if (Sistema.UsuarioLogado.IsFavorito(midia))
                {
                    Sistema.UsuarioLogado.RemoverFavorito(midia);
                    MessageBox.Show($"'{midia.Nome}' removido dos favoritos!", "Favoritos");
                }
                else
                {
                    Sistema.UsuarioLogado.AdicionarFavorito(midia);
                    MessageBox.Show($"'{midia.Nome}' adicionado aos favoritos!", "Favoritos");
                }
                AtualizarBotaoFavorito(btnFavorito);
            };
            
            this.Controls.Add(txtDetalhes);
            this.Controls.Add(btnFavorito);
            this.Controls.Add(lblTitulo);
        }

        private void AtualizarBotaoFavorito(Button btn)
        {
            if (Sistema.UsuarioLogado == null)
            {
                btn.Text = "Faça login para favoritar";
                btn.BackColor = Color.Gray;
                btn.Enabled = false;
            }
            else if (Sistema.UsuarioLogado.IsFavorito(midia))
            {
                btn.Text = "★ Remover dos Favoritos";
                btn.BackColor = Color.DarkRed;
                btn.Enabled = true;
            }
            else
            {
                btn.Text = "☆ Adicionar aos Favoritos";
                btn.BackColor = Color.DarkRed;
                btn.Enabled = true;
            }
            btn.ForeColor = Color.White;
        }
    }

    public class FavoritosForm : Form
    {
        public FavoritosForm()
        {
            this.Text = "Meus Favoritos";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.BackColor = Color.Black;
            flowPanel.AutoScroll = true;
            flowPanel.WrapContents = true;
            flowPanel.Padding = new Padding(20);
            this.Controls.Add(flowPanel);

            if (Sistema.UsuarioLogado == null)
            {
                Label lblLogin = new Label();
                lblLogin.Text = "Faça login para ver seus favoritos";
                lblLogin.ForeColor = Color.White;
                lblLogin.Font = new Font("Arial", 14, FontStyle.Bold);
                lblLogin.Dock = DockStyle.Fill;
                lblLogin.TextAlign = ContentAlignment.MiddleCenter;
                flowPanel.Controls.Add(lblLogin);
                return;
            }

            if (Sistema.UsuarioLogado.Favoritos.Count == 0)
            {
                Label lblVazio = new Label();
                lblVazio.Text = "Nenhum favorito adicionado";
                lblVazio.ForeColor = Color.White;
                lblVazio.Font = new Font("Arial", 14, FontStyle.Bold);
                lblVazio.Dock = DockStyle.Fill;
                lblVazio.TextAlign = ContentAlignment.MiddleCenter;
                flowPanel.Controls.Add(lblVazio);
                return;
            }

            foreach (var midia in Sistema.UsuarioLogado.Favoritos)
            {
                Panel card = CriarCardMidia(midia);
                flowPanel.Controls.Add(card);
            }
        }

        private Panel CriarCardMidia(Midia midia)
        {
            Panel panel = new Panel();
            panel.Width = 250;
            panel.Height = 150;
            panel.BackColor = Color.FromArgb(30, 30, 30);
            panel.Margin = new Padding(10);
            panel.Padding = new Padding(10);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Cursor = Cursors.Hand;

            // Indicador de favorito
            Panel favoritoIndicator = new Panel();
            favoritoIndicator.Size = new Size(20, 20);
            favoritoIndicator.Location = new Point(panel.Width - 30, 10);
            favoritoIndicator.BackColor = Color.Gold;
            favoritoIndicator.BorderStyle = BorderStyle.FixedSingle;

            Label titulo = new Label();
            titulo.Text = midia.Nome;
            titulo.ForeColor = Color.Red;
            titulo.Font = new Font("Arial", 12, FontStyle.Bold);
            titulo.Dock = DockStyle.Top;
            titulo.Height = 25;

            Label tipo = new Label();
            tipo.Text = midia is Filme ? "[FILME]" : "[SÉRIE]";
            tipo.ForeColor = Color.White;
            tipo.Dock = DockStyle.Top;
            tipo.Height = 20;

            Label info = new Label();
            info.Text = $"{midia.Ano} | {midia.Genero}";
            info.ForeColor = Color.White;
            info.Dock = DockStyle.Fill;

            panel.Controls.Add(favoritoIndicator);
            panel.Controls.Add(info);
            panel.Controls.Add(tipo);
            panel.Controls.Add(titulo);

            panel.Click += (s, e) =>
            {
                new DetalhesForm(midia).ShowDialog();
            };

            return panel;
        }
    }

    public class MainForm : Form
    {
        private ToolStrip toolStrip;
        private ToolStripLabel lblUsuario;
        private FlowLayoutPanel flowPanel;
        private ToolStripButton btnFavoritos;

        public MainForm()
        {
            this.Text = "Netflix Simples";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.WindowState = FormWindowState.Maximized;

            // Configurar barra de ferramentas
            toolStrip = new ToolStrip();
            toolStrip.BackColor = Color.FromArgb(20, 20, 20);
            toolStrip.ForeColor = Color.White;
            toolStrip.RenderMode = ToolStripRenderMode.System;
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;

            // Botão de login
            var btnLogin = new ToolStripButton("Login");
            btnLogin.Click += (s, e) => FazerLogin();
            toolStrip.Items.Add(btnLogin);

            // Botão de cadastro de usuário
            var btnCadastrarUsuario = new ToolStripButton("Novo Usuário");
            btnCadastrarUsuario.Click += (s, e) => new CadastroUsuarioForm().ShowDialog();
            toolStrip.Items.Add(btnCadastrarUsuario);

            // Botão de cadastro de mídia
            var btnCadastrarMidia = new ToolStripButton("Cadastrar Mídia");
            btnCadastrarMidia.Click += (s, e) =>
            {
                var form = new CadastroMidiaForm();
                form.FormClosed += (s2, e2) => CarregarMidias();
                form.ShowDialog();
            };
            toolStrip.Items.Add(btnCadastrarMidia);

            // Botão de favoritos
            btnFavoritos = new ToolStripButton("Favoritos");
            btnFavoritos.Click += (s, e) => AbrirFavoritos();
            btnFavoritos.Enabled = Sistema.UsuarioLogado != null;
            toolStrip.Items.Add(btnFavoritos);

            // Separador
            toolStrip.Items.Add(new ToolStripSeparator());

            // Label do usuário
            lblUsuario = new ToolStripLabel();
            AtualizarStatusUsuario();
            toolStrip.Items.Add(lblUsuario);

            this.Controls.Add(toolStrip);

            // Configurar painel de conteúdo
            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                BackColor = Color.Black,
                Padding = new Padding(20)
            };
            this.Controls.Add(flowPanel);

            // Carregar dados iniciais
            CarregarMidias();
        }

        private void FazerLogin()
        {
            var loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                AtualizarStatusUsuario();
                btnFavoritos.Enabled = true;
                CarregarMidias();
            }
        }

        private void AbrirFavoritos()
        {
            if (Sistema.UsuarioLogado == null)
            {
                MessageBox.Show("Faça login para acessar seus favoritos!", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            new FavoritosForm().ShowDialog();
            CarregarMidias(); // Recarregar após possível alteração de favoritos
        }

        private void AtualizarStatusUsuario()
        {
            if (Sistema.UsuarioLogado != null)
            {
                lblUsuario.Text = $"Usuário: {Sistema.UsuarioLogado.Nome}";
            }
            else
            {
                lblUsuario.Text = "Não logado";
            }
        }

        private void CarregarMidias()
        {
            flowPanel.Controls.Clear();
            
            foreach (var midia in Sistema.Midias)
            {
                Panel card = CriarCardMidia(midia);
                flowPanel.Controls.Add(card);
            }
        }

        private Panel CriarCardMidia(Midia midia)
        {
            Panel panel = new Panel();
            panel.Width = 250;
            panel.Height = 150;
            panel.BackColor = Color.FromArgb(30, 30, 30);
            panel.Margin = new Padding(10);
            panel.Padding = new Padding(10);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Cursor = Cursors.Hand;

            // Indicador de favorito
            Panel favoritoIndicator = new Panel();
            favoritoIndicator.Size = new Size(20, 20);
            favoritoIndicator.Location = new Point(panel.Width - 30, 10);
            favoritoIndicator.BackColor = Sistema.UsuarioLogado != null && Sistema.UsuarioLogado.IsFavorito(midia) ? 
                Color.Gold : Color.Gray;
            favoritoIndicator.BorderStyle = BorderStyle.FixedSingle;

            Label titulo = new Label();
            titulo.Text = midia.Nome;
            titulo.ForeColor = Color.Red;
            titulo.Font = new Font("Arial", 12, FontStyle.Bold);
            titulo.Dock = DockStyle.Top;
            titulo.Height = 25;

            Label tipo = new Label();
            tipo.Text = midia is Filme ? "[FILME]" : "[SÉRIE]";
            tipo.ForeColor = Color.White;
            tipo.Dock = DockStyle.Top;
            tipo.Height = 20;

            Label info = new Label();
            info.Text = $"{midia.Ano} | {midia.Genero}";
            info.ForeColor = Color.White;
            info.Dock = DockStyle.Fill;

            panel.Controls.Add(favoritoIndicator);
            panel.Controls.Add(info);
            panel.Controls.Add(tipo);
            panel.Controls.Add(titulo);

            panel.Click += (s, e) =>
            {
                var detalhesForm = new DetalhesForm(midia);
                detalhesForm.FormClosed += (sender, args) => 
                {
                    // Atualizar o indicador de favorito após fechar os detalhes
                    favoritoIndicator.BackColor = Sistema.UsuarioLogado != null && Sistema.UsuarioLogado.IsFavorito(midia) ? 
                        Color.Gold : Color.Gray;
                };
                detalhesForm.ShowDialog();
            };

            return panel;
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
