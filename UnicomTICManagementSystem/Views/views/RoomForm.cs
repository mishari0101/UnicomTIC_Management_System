using System;
using System.Windows.Forms;
using UnicomTICManagementSystem.Controllers;

namespace UnicomTICManagementSystem.Views
{
    public partial class RoomForm : Form
    {
        private readonly RoomController _controller;

        public RoomForm()
        {
            InitializeComponent();
            _controller = new RoomController();
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {
            LoadRooms();
        }

        private void LoadRooms()
        {
            dgvRooms.DataSource = _controller.GetAllRooms();
            dgvRooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // --- VALIDATION START ---
            if (string.IsNullOrWhiteSpace(txtRoomName.Text) || string.IsNullOrWhiteSpace(txtRoomType.Text))
            {
                MessageBox.Show("Please enter both a room name and a room type.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // --- VALIDATION END ---

            _controller.AddRoom(txtRoomName.Text, txtRoomType.Text);
            MessageBox.Show("Room added successfully!");
            LoadRooms();
            txtRoomName.Clear();
            txtRoomType.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvRooms.CurrentRow == null)
            {
                MessageBox.Show("Please select a room from the grid to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- VALIDATION START ---
            if (string.IsNullOrWhiteSpace(txtRoomName.Text) || string.IsNullOrWhiteSpace(txtRoomType.Text))
            {
                MessageBox.Show("Please enter both a room name and a room type.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // --- VALIDATION END ---

            int roomId = Convert.ToInt32(dgvRooms.CurrentRow.Cells["RoomID"].Value);
            _controller.UpdateRoom(roomId, txtRoomName.Text, txtRoomType.Text);
            MessageBox.Show("Room updated successfully!");
            LoadRooms();
            txtRoomName.Clear();
            txtRoomType.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRooms.CurrentRow == null) return;
            int roomId = Convert.ToInt32(dgvRooms.CurrentRow.Cells["RoomID"].Value);
            _controller.DeleteRoom(roomId);
            LoadRooms();
            txtRoomName.Clear();
            txtRoomType.Clear();
        }

        private void dgvRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRooms.CurrentRow != null)
            {
                txtRoomName.Text = dgvRooms.CurrentRow.Cells["RoomName"].Value.ToString();
                txtRoomType.Text = dgvRooms.CurrentRow.Cells["RoomType"].Value.ToString();
            }
        }
    }
}