#include "stm32f30x_gpio.h"
#include "stm32f30x_rcc.h"
#include "stm32f30x_exti.h"
#include "stm32f30x_syscfg.h"
#include "stm32f30x_misc.h"
#include "stm32f30x.h"
#include "stm32f30x_usart.h"

#define DELAY_TIME	    		60000
#define DELAY_TIME1	    		90000000
#define BAUDRATE 9600


GPIO_InitTypeDef GPIO_InitStructure;
GPIO_InitTypeDef GPIO_InitStructureA;
GPIO_InitTypeDef GPIO_InitStructureB;
NVIC_InitTypeDef NVIC_InitStructure;
//UART
USART_InitTypeDef usart;
GPIO_InitTypeDef uart_port;
void down_r();
void down_l();
void up_r();
void up_l();

void init()		
{
    RCC_AHBPeriphClockCmd(RCC_AHBPeriph_GPIOE, ENABLE);
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8 | GPIO_Pin_9;	
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_OUT;	
    GPIO_Init(GPIOE, &GPIO_InitStructure);
	    //
	RCC_AHBPeriphClockCmd(RCC_AHBPeriph_GPIOA, ENABLE);
    GPIO_InitStructureA.GPIO_Speed = GPIO_Speed_2MHz;
    GPIO_InitStructureA.GPIO_Pin = GPIO_Pin_0 | GPIO_Pin_3;	
    GPIO_InitStructureA.GPIO_Mode = GPIO_Mode_OUT;	
		GPIO_InitStructureA.GPIO_OType = GPIO_OType_OD;
		GPIO_InitStructureA.GPIO_PuPd = GPIO_PuPd_UP ;
    GPIO_Init(GPIOA, &GPIO_InitStructureA);	
	//
		RCC_AHBPeriphClockCmd(RCC_AHBPeriph_GPIOB, ENABLE);
    GPIO_InitStructureB.GPIO_Speed = GPIO_Speed_2MHz;
    GPIO_InitStructureB.GPIO_Pin = GPIO_Pin_0 | GPIO_Pin_2;	
    GPIO_InitStructureB.GPIO_Mode = GPIO_Mode_OUT;	
		GPIO_InitStructureB.GPIO_OType = GPIO_OType_OD;
		GPIO_InitStructureB.GPIO_PuPd = GPIO_PuPd_UP ;
    GPIO_Init(GPIOB, &GPIO_InitStructureB);	
		
		 
    /// PA9 - TX PA10 -RX
    ///USART
		RCC_APB2PeriphClockCmd(RCC_APB2Periph_USART1, ENABLE);
    GPIO_StructInit(&uart_port);
    uart_port.GPIO_Mode = GPIO_Mode_AF;
		uart_port.GPIO_OType = GPIO_OType_PP;
    uart_port.GPIO_Pin = GPIO_Pin_9 | GPIO_Pin_10;
    uart_port.GPIO_Speed = GPIO_Speed_2MHz;
    GPIO_Init(GPIOA, &uart_port);
 
 //   uart_port.GPIO_Mode = GPIO_Mode_IN_FLOATING;
//		uart_port.GPIO_Mode = GPIO_Mode_AF;
//		uart_port.GPIO_OType = GPIO_OType_PP;
//		uart_port.GPIO_PuPd = GPIO_PuPd_NOPULL;
//    uart_port.GPIO_Pin = GPIO_Pin_10;
//    uart_port.GPIO_Speed = GPIO_Speed_2MHz;
//    GPIO_Init(GPIOA, &uart_port);

//  GPIO_PinAFConfig(GPIOA, GPIO_PinSource9, GPIO_AF_USART1); //
//  GPIO_PinAFConfig(GPIOA, GPIO_PinSource10, GPIO_AF_USART1);
GPIO_PinAFConfig(GPIOA, GPIO_PinSource10, GPIO_AF_7);
GPIO_PinAFConfig(GPIOA, GPIO_PinSource9, GPIO_AF_7);
   
    USART_StructInit(&usart);
    usart.USART_BaudRate = BAUDRATE;
		  
  usart.USART_WordLength = USART_WordLength_8b;// we want the data frame size to be 8 bits (standard)
  usart.USART_StopBits = USART_StopBits_1;		// we want 1 stop bit (standard)
  usart.USART_Parity = USART_Parity_No;		// we don't want a parity bit (standard)
  usart.USART_HardwareFlowControl = USART_HardwareFlowControl_None; // we don't want flow control (standard)
  usart.USART_Mode = USART_Mode_Tx | USART_Mode_Rx; // we want to enable the transmitter and the receiver
  USART_Init(USART1, &usart);
}

void up_l()
{
		GPIO_ResetBits(GPIOA, GPIO_Pin_0); //Input2
//		GPIO_SetBits(GPIOA, GPIO_Pin_1); //Enable
		GPIO_SetBits(GPIOA, GPIO_Pin_3); //Input1
}

void down_l()
{
		GPIO_SetBits(GPIOA, GPIO_Pin_0); 
//		GPIO_SetBits(GPIOA, GPIO_Pin_1);
		GPIO_ResetBits(GPIOA, GPIO_Pin_3);
}

void up_r()
{
		GPIO_ResetBits(GPIOB, GPIO_Pin_0); //Input3
//		GPIO_SetBits(GPIOB, GPIO_Pin_1); //Enable
		GPIO_SetBits(GPIOB, GPIO_Pin_2); //Input4
}

void down_r()
{
		GPIO_SetBits(GPIOB, GPIO_Pin_0); 
//		GPIO_SetBits(GPIOB, GPIO_Pin_1);
		GPIO_ResetBits(GPIOB, GPIO_Pin_2);
}

void resetAll()
{
	GPIO_ResetBits(GPIOB, GPIO_Pin_2);
//	GPIO_ResetBits(GPIOB, GPIO_Pin_1);
	GPIO_ResetBits(GPIOB, GPIO_Pin_0);
	GPIO_ResetBits(GPIOA, GPIO_Pin_0);
//	GPIO_ResetBits(GPIOA, GPIO_Pin_1);
	GPIO_ResetBits(GPIOA, GPIO_Pin_3);
}

void delay(unsigned long p)
{
	unsigned long i;
	for(i=0;i<p ;i++);
}

int main()
{

	init();
	    __enable_irq ();
    USART_ITConfig(USART1, USART_IT_RXNE, ENABLE);
	  NVIC_InitStructure.NVIC_IRQChannel = USART1_IRQn;		 // we want to configure the USART1 interrupts
		NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 0;// this sets the priority group of the USART1 interrupts
		NVIC_InitStructure.NVIC_IRQChannelSubPriority = 0;		 // this sets the subpriority inside the group
		NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			 // the USART1 interrupts are globally enabled

    USART_Cmd(USART1, ENABLE);
		GPIO_ResetBits(GPIOE, GPIO_Pin_9);
		NVIC_EnableIRQ(USART1_IRQn);
		while(1)
		{
		}

}

void USART1_IRQHandler(void)
{	
	uint16_t usartData = 0;
	uint16_t tmp = USART_GetITStatus(USART1, USART_IT_RXNE);

	usartData = USART_ReceiveData(USART1);
    if (usartData)
    {
//			usartData = USART_ReceiveData(USART1);
			if(usartData==0x0035)		
			{
				GPIO_SetBits(GPIOE, GPIO_Pin_9);
			}
			if(usartData==0x0038)		
			{
				resetAll();
				GPIO_SetBits(GPIOE, GPIO_Pin_8);
				up_l();
				up_r();
			}
			if(usartData==0x0032)		
			{
				resetAll();
				GPIO_SetBits(GPIOE, GPIO_Pin_8);
				down_l();
				down_r();
			}
			if(usartData==0x0034)		
			{
				resetAll();
				up_l();
			}
			if(usartData==0x0036)		
			{
				resetAll();
				up_r();
			}
			if(usartData==0x0030)		
			{
				resetAll();
			}
			
    }
}
