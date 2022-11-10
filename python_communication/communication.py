import sys,serial,time,os,json
import serial.tools.list_ports
from PyQt5.QtCore import *  #pip install pyqt5
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5 import QtCore
# import pyqtgraph as pg

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

    def send(self, data_to_send):
        cmd = bytes.fromhex(data_to_send)
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
        self.setGeometry(200,200,1200,800)
        title = 'COM communication with 38DLP'
        self.setWindowTitle(title)

        self.create_main_frame()
        self.main_frame.show()
        self.Device_thread = Device()
        self.Device_thread.response_string.connect(self.listen_data_from_Device)

    def create_main_frame(self):
        self.main_frame = QWidget()

        self.command_layout = QGridLayout()

        self.gage_info_button = QPushButton(text="Ask GageInfo")

        self.command_layout.addWidget(self.gage_info_button,0,0,1,1)
        

   
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
        self.gage_info_button.clicked.connect(self.send_gage_info)

        
    def send_gage_info(self):
        cmd = "GAGEINFO?\r\n"
        print(f'Sending command for asking Gage Info response')
        print(f'Origonal Command is {cmd}')
        try:
            self.Device_thread.send(data_to_send=cmd)
        except Exception as error:
            print(f'Cannot send due to\n{error}')

    def listen_data_from_Device(self,data):
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