import sys,serial,time,os,json
import serial.tools.list_ports
from PyQt5.QtCore import *  #pip install pyqt5
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5 import QtCore
# import pyqtgraph as pg
''''
This is a example to try to establish and test the communication between PC and 38DLP under Python
Relys on the pyserial to establish the communication
'''
class Device(QThread):
    com = 'com3'
    port = 4800
    port_list = []
    response_string = pyqtSignal(str)

    def __init__(self):
        super(Device, self).__init__()
        com_available = self.get_port_list()
        if com_available:
            print(self.port_list)
        else:
            print(len(self.port_list) )
            print("No Available COM")
            sys.exit(1)
        self.create_communication(self.com,self.port)
        # self.timer = QTimer(self)
        # self.timer.timeout.connect()

    def get_port_list(self):
        print('Verify the Port data')
        self.port_list = [serial.tools.list_ports.comports()]
        print(f'The port list is {self.port_list}')

        if len(self.port_list) == 0:
            return False
        elif len(self.port_list) > 0:
            return True
            
    def create_communication(self, com, port):
        self.serl = serial.Serial(com, port, timeout = 3)

    def recv_data(self, ser):
        ser = self.serl
        data = ''    
        if serial.inWaiting() > 0:
            print(serial.inWaiting())
            print('==========================================')
            data += str(serial.read_all())

            print('==========================================')
            print(data)
            print(f'Data {data}')
            self.response_string.emit(data)
        else:
            pass

    def send(self, data_to_send):
        # print('Device Thread')
        cmd = bytes(data_to_send,'utf-8')
        print(f'After process to send: {cmd}')
        self.serl.write(cmd)

    def listen(self):
        self.recv_data()

    def run(self):
        self.listen()
    
    def close_connection(self):
        self.serl.close()



class Main(QMainWindow):

    def __init__(self,parent=None):
        super(Main, self).__init__()

        QMainWindow.__init__(self, parent)
        self.setGeometry(500,500,200,400)
        title = 'COM communication with 38DLP'
        self.setWindowTitle(title)

        self.create_main_frame()
        self.main_frame.show()
        # self.Device_thread = Device()
        # self.Device_thread.response_string.connect(self.listen_data_from_Device)



    def create_main_frame(self):
        self.main_frame = QWidget()

        self.command_layout = QGridLayout()
        self.multi_character_command_line = QLabel(text= 'Multi-Character Commands')
        
        self.hook_device_button = QPushButton(text= 'Connect 38DLP')
        self.gage_info_button = QPushButton(text="Ask GageInfo")
        self.FTP_block_info_button = QPushButton(text = 'FTP Block Info')
        self.file_dirctory_button = QPushButton(text = 'File Directroy')
        self.file_read_button = QPushButton(text= 'File Read')
        self.file_wirte_button = QPushButton(text= 'File Write')
        self.file_create_button = QPushButton(text = 'File Create')
        self.file_delete_button = QPushButton(text = "File Delete")
        self.application_setup_directory_button = QPushButton(text= 'Application Setup Directory')
        self.application_setup_read_button = QPushButton(text= 'Application Setup Read')
        self.application_setup_write_button = QPushButton(text = 'Application Setup Write')
        self.make_application_setup_active_buton = QPushButton(text= ' Make Application Setup Active')
        self.transducer_list_button = QPushButton(text= 'Transducer List')
        self.database_memory_status_button = QPushButton(text='Database Memory Status')
        self.ID_data_send_button = QPushButton(text='ID Data Send')
        self.version_get_button = QPushButton(text='Version Get')
        self.units_get_button = QPushButton(text='Unit Get')
        self.velocity_get_button = QPushButton(text='Velocity Get')
        self.mode_get_button = QPushButton(text= 'Mode Get')
        self.data_window1_get_button = QPushButton(text = 'Data Window1 Get')
        self.data_window2_get_button = QPushButton(text = 'Data Window2 Get')
        self.sample_rate_get_button = QPushButton(text= 'Sample Rate Get')
        self.range_send_button = QPushButton(text= 'Range Send')
        self.battery_level_get_button = QPushButton(text= 'Battery Level Get')
        self.veloccity_set_button = QPushButton(text= 'Velocity Set')
        self.communication_protocol_change_button = QPushButton(text= 'Communication Protocol Change')
        self.get_setup_name_button = QPushButton(text= 'Get Setup Name')
        self.set_setup_name_button = QPushButton(text= 'Set Setup Name')

        self.single_character_command_line = QLabel(text= 'Single Character Command')
        






        self.command_layout.addWidget(self.hook_device_button,0,0,1,3)
        self.command_layout.addWidget(self.gage_info_button,1,0,1,1)
        self.command_layout.addWidget(self.FTP_block_info_button,1,1,1,1)
        self.command_layout.addWidget(self.file_dirctory_button,1,2,1,1)
        self.command_layout.addWidget(self.file_read_button,2,0,1,1)
        self.command_layout.addWidget(self.file_wirte_button,2,1,1,1)
        self.command_layout.addWidget(self.file_delete_button,2,2,1,1)
        self.command_layout.addWidget(self.application_setup_directory_button,3,0,1,1)
        self.command_layout.addWidget(self.application_setup_read_button,3,1,1,1)
        self.command_layout.addWidget(self.application_setup_write_button,3,2,1,1)
        self.command_layout.addWidget(self.make_application_setup_active_buton,4,0,1,3)
        self.command_layout.addWidget(self.transducer_list_button,5,0,1,1)
        self.command_layout.addWidget(self.database_memory_status_button,5,1,1,1)
        self.command_layout.addWidget(self.ID_data_send_button,5,2,1,1)
        self.command_layout.addWidget(self.version_get_button,6,0,1,1)

        self.command_layout.addWidget(self.units_get_button,6,1,1,1)
        self.command_layout.addWidget(self.velocity_get_button,6,2,1,1)
        self.command_layout.addWidget(self.mode_get_button,7,0,1,1)
        self.command_layout.addWidget(self.data_window1_get_button,7,1,1,1)
        self.command_layout.addWidget(self.data_window2_get_button,7,2,1,1)
        self.command_layout.addWidget(self.sample_rate_get_button,8,0,1,1)
        self.command_layout.addWidget(self.battery_level_get_button,8,1,1,1)
        self.command_layout.addWidget(self.veloccity_set_button,8,2,1,1)
        self.command_layout.addWidget(self.communication_protocol_change_button,9,0,1,3)
        self.command_layout.addWidget(self.get_setup_name_button,10,0,1,1)
        self.command_layout.addWidget(self.set_setup_name_button,10,1,1,1)


        

   
        """
        Combine everything 
        """

        self.hbox = QHBoxLayout()

        self.hbox.addLayout(self.command_layout)

        self.main_frame.setLayout(self.hbox)
        self.setCentralWidget(self.main_frame)

        """
        Next we will link all the button to a function
        """
        self.hook_device_button.clicked.connect(self.initialize_device)
        self.gage_info_button.clicked.connect(self.send_gage_info)
        self.FTP_block_info_button.clicked.connect(self.ftp_block_info)
        self.file_dirctory_button.clicked.connect(self.file_directory)
        self.file_read_button.clicked.connect(self.file_read)
        self.file_wirte_button.clicked.connect(self.file_write)
        self.file_create_button.clicked.connect(self.file_create)
        self.file_delete_button.clicked.connect(self.file_delete)
        self.application_setup_directory_button.clicked.connect(self.application_Setup_Directory)
        self.application_setup_read_button.clicked.connect(self.application_Setup_Read)
        self.application_setup_write_button.clicked.connect(self.application_Setup_Write)
        self.make_application_setup_active_buton.clicked.connect(self.make_application_setup_Active)
        self.transducer_list_button.clicked.connect(self.transducer_List)
        self.database_memory_status_button.clicked.connect(self.database_Memory_Status)
        self.ID_data_send_button.clicked.connect(self.ID_Data_Send)
        self.version_get_button.clicked.connect(self.version_get)
        self.units_get_button.clicked.connect(self.units_get)
        self.velocity_get_button.clicked.connect(self.velocity_get)
        self.mode_get_button.clicked.connect(self.mode_get)
        self.data_window1_get_button.clicked.connect(self.data_Window1_Get)
        self.data_window2_get_button.clicked.connect(self.data_Window2_Get)
        self.sample_rate_get_button.clicked.connect(self.sample_rate_get)
        # self.range_send_button.clicked.connect()
        self.battery_level_get_button.clicked.connect(self.battery_level_get)
        self.veloccity_set_button.clicked.connect(self.velocity_set)
        self.communication_protocol_change_button.clicked.connect(self.communication_Protocol_change)
        self.get_setup_name_button.clicked.connect(self.get_setup_name)
        self.set_setup_name_button.clicked.connect(self.set_Setup_Name)




    # =====================================================================================
    def initialize_device(self):
        self.Device_thread = Device()
        self.Device_thread.response_string.connect(self.listen_data_from_Device)


    def send_multi_command(self,cmd):
        print(f'The Command is:  {cmd}')
        try:
            self.Device_thread.send(data_to_send=cmd)
        except Exception as error:
            print(f'Cannot send due to\n{error}')
        
    def send_gage_info(self):
        command = "GAGEINFO?\r\n"
        print(f'Sending command for asking Gage Info response')
        print(f'Origonal Command is {command}')
        self.send_multi_command(cmd=command)

    def ftp_block_info(self):
        command = "FTPINFO?\r\n"
        self.send_multi_command(cmd=command)
        
    def file_directory(self):
        command = "FILEDIR?\r\n"
        self.send_multi_command(command)

    def file_read(self):
        command = " FILEREAD?\2\r\n"
        self.send_multi_command(command)

    def file_write(self):
        command = "FILEWRITE=\2\r\n"
        self.send_multi_command(command)

    def file_create(self):
        command = " FILECREATE=\2\r\n"
        self.send_multi_command(command)
    
    def file_delete(self):
        command = "FILEDELETE\2\r\n"
        self.send_multi_command(command)

    def application_Setup_Directory(self):
        command = 'APPSUDIR?\r\n'
        self.send_multi_command(command)
    
    def application_Setup_Read(self):
        command = "APPSUREAD?\2\r\n"
        self.send_multi_command(command)

    def application_Setup_Write(self):
        command = "APPSUWRITE=\2\r\n"
        self.send_multi_command(command)

    def make_application_setup_Active (self):
        command = "APPSUACTIVE=\2\r\n"
        self.send_multi_command(command)

    def transducer_List (self):
        command = "XDCRLIST?\r\n"
        self.send_multi_command(command)

    def database_Memory_Status (self):
        command = "MEMORY?\r\n"
        self.send_multi_command(command)

    def ID_Data_Send(self):
        command =  "SEND=SINGLE\r\n"
        self.send_multi_command(command)

    def version_get(self):
        command = "VER?\r\n"
        self.send_multi_command(command)

    def units_get(self):
        command = "UNITS?\r\n"
        self.send_multi_command(command)

    def velocity_get(self):
        command = "VELOCITY?\r\n"
        self.send_multi_command(command)

    def mode_get(self):
        command = 'MODE?\r\n'
        self.send_multi_command(command)
    
    def data_Window1_Get(self):
        command = "DATAWIN1?\r\n"
        self.send_multi_command(command)

    def data_Window2_Get(self):
        command = "DATAWIN2?\r\n"
        self.send_multi_command(command)
    
    def sample_rate_get(self):
        command = "SRATE?\r\n"
        self.send_multi_command(command)
    
    # def range_send(self):
    #     command = ""
    def battery_level_get(self):
        command= "BATTLEVEL?\r\n"
        self.send_multi_command(command)

    def velocity_set(self):
        value = 1000
        command = f"VELOCITY={value}\r\n"
        self.send_multi_command(command)
    
    def communication_Protocol_change(self):
        command = "PROTO=SINGLE\r\n"
        self.send_multi_command(command)

    def get_setup_name(self):
        command = "SETUPNAME?\r\n"
        self.send_multi_command(command)
    
    def set_Setup_Name(self):
        """
        This command is valid for single element transducer setups only.
        """
        name = 'LOL'
        command = f"ETUPNAME={name}\r\n"  
        self.send_multi_command(command)

    
    

    

    
    # ==================================================================================

    def listen_data_from_Device(self,data):
        
        '''
        Let's just print the response first and we will see what will happen next
        '''
        print("Received:")
        print(data)

def run_application():
    app = QApplication(sys.argv)
    form = Main()
    form.show()
    app.exec_()


if __name__ == '__main__':
    run_application()
    print("ENDENDENDENDENDENDENDENDENDENDENDEND")