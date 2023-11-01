;#DropDB NucleusEasyTest

#Application NucleusWin
#Application NucleusWeb

*FillForm
 User Name = Admin
*Action Log In

#IfDef NucleusWin

*Action About...
 
*Action OK

#EndIf

#IfDef NucleusWeb
*Action Navigation(My Details)
#EndIf
