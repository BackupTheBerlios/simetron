package com.olympum.cibeles.engine.core;

public final class SimpleCalculator {
		public SimpleCalculator(Integer mybase) {
			this.mybase = mybase;
		}

		//to make it testable this returns always the same
		public Integer generateRandom() {
			return new Integer(mybase.intValue()/2+1);
		}

		public Integer sum(Integer a, Integer b) {
			return new Integer(a.intValue() + b.intValue());
		}

		public Integer mybase;
	}
