DES
===

An implementation of DES in C#

This was created as a learning exercise, do not use it for a real application.

I relied heavily on J. Orlin Grabbe's documentation and test data at
[http://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm](http://page.math.tu-berlin.de/~kant/teaching/hess/krypto-ws2006/des.htm)

To avoid too many casts most values are stored in ulongs left aligned, so the byte 0xAB will be stored as 0xAB00000000000000.

There is no implementation of a mode of operation yet.
